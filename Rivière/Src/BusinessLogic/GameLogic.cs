using System;
using System.Collections.Generic;
using System.Linq;
using Rivière.Utils;


namespace Rivière.BusinessLogic
{
    public partial class GameLogic
    {
        /// <summary>
        /// The current round or step of the game.
        /// </summary>
        public GameState GameState { get; private set; }

        /// <summary>
        /// The player drawing the next card. This returns non-null only during the "asking" rounds.
        /// Indeed, except when players play one after the other during the first four rounds, there 
        /// is no "current player".
        /// </summary>
        public Player CurrentPlayer {
            get
            {
                if (GameState == GameState.ASKING_FOR_COLOR
                    || GameState == GameState.ASKING_FOR_LESS_EQUAL_MORE
                    || GameState == GameState.ASKING_FOR_INTER_EQUAL_EXTER
                    || GameState == GameState.ASKING_FOR_SUIT)
                {
                    return players[currentPlayerIdx];
                }
                else return null;
            }
        }

        /// <summary>
        /// The current list of players
        /// </summary>
        public readonly List<Player> players = new List<Player>();
        
        /// <summary>
        /// The card deck
        /// </summary>
        public readonly CardDeck cardDeck = new CardDeck();

        /// <summary>
        /// The special rules of the game. It is possible to set rules while the gamestate
        /// is NOT_STARTED or WAITING_FOR_PLAYER or WAITING_FOR_START.
        /// </summary>
        public SpecialRules SpecialRules
        {
            get => specialRules;
            set
            {
                if (GameState != GameState.NOT_STARTED
                    && GameState != GameState.WAITING_FOR_PLAYER
                    && GameState != GameState.WAITING_FOR_START)
                {
                    throw new GameStateUnauthorizedAction("Setting game rules", GameState);
                }
                specialRules = value;
            }
        }

        /// <summary>
        /// The maximum number of players to start a game.
        /// </summary>
        public const int maxPlayers = 10;

        /// <summary>
        /// The minimum number of players to start a game.
        /// </summary>
        public const int minPlayers = 4;

        /// <summary>
        /// The number of sips during the death card. It is important because once we reach
        /// the death card, the number of sips during last round goes back to 1, until the 
        /// deck is empty.
        /// </summary>
        public const int numberOfSipsDeathCard = 6;

        private SpecialRules specialRules = new SpecialRules();

        /// <summary>
        /// The index of the current player used to get the current player.
        /// </summary>
        int currentPlayerIdx = 0;

        /// <summary>
        /// Used during the last round of the game. Do the players give or take sips?
        /// </summary>
        bool lastRoundIsGiving = true;

        /// <summary>
        /// Used during the last round of the game. How many sips for this draw?
        /// </summary>
        int lastRoundNumberOfSips = 1;

        public GameLogic()
        {
            GameState = GameState.NOT_STARTED;
        }

        /// <summary>
        /// Start the preparation of the game.
        /// Expected state: NOT_STARTED
        /// Output state: WAITING_FOR_PLAYER
        /// </summary>
        public void PrepareGame()
        {
            if (GameState != GameState.NOT_STARTED)
            {
                throw new GameStateUnauthorizedAction("Prepare the game", GameState);
            }

            GameState = GameState.WAITING_FOR_PLAYER;
            OnGamePreparation?.Invoke();
        }

        /// <summary>
        /// Add a new player to the list. The given name must be available.
        /// Expected state: WAITING_FOR_PLAYER
        /// </summary>
        /// <param name="playerName">The name of the new player.</param>
        public void AddPlayer(string playerName)
        {
            if (GameState != GameState.WAITING_FOR_PLAYER)
            {
                throw new GameStateUnauthorizedAction("Add a player", GameState);
            }

            if (players.Where(p => p.Name == playerName).Any())
            {
                OnPlayerNameAlreadyTaken(playerName);
                return;
            }

            players.Add(new Player(playerName));
            OnPlayerAdded?.Invoke(playerName);
        }

        /// <summary>
        /// Remove a player from the list, given its name.
        /// Expected state: WAITING_FOR_PLAYER
        /// </summary>
        /// <param name="playerName">The name of the player to remove.</param>
        public void RemovePlayer(string playerName)
        {
            if (GameState != GameState.WAITING_FOR_PLAYER)
            {
                throw new GameStateUnauthorizedAction("Remove a player", GameState);
            }

            Player playerToRemove = players.Where(p => p.Name == playerName).FirstOrDefault();
            if (playerToRemove != null)
            {
                players.Remove(playerToRemove);
                OnPlayerRemoved?.Invoke(playerName);
            }
        }

        /// <summary>
        /// Validate the list of player and set the game as ready to start
        /// Expected state: WAITING_FOR_PLAYER
        /// Output state: WAITING_FOR_START
        /// </summary>
        public void ValidatePlayer()
        {
            if (GameState != GameState.WAITING_FOR_PLAYER)
            {
                throw new GameStateUnauthorizedAction("Prepare the game", GameState);
            }

            if (players.Count() > maxPlayers)
            {
                OnTooManyPlayers?.Invoke();
                OnPlayerValidated?.Invoke(false);
                return;
            }
            else if (players.Count < minPlayers)
            {
                OnNotEnoughPlayers?.Invoke();
                OnPlayerValidated?.Invoke(false);
                return;
            }

            // Randomize player order if the rule is set
            if (SpecialRules.ArePlayersShuffled)
                players.Shuffle();

            GameState = GameState.WAITING_FOR_START;
            OnPlayerValidated?.Invoke(true);
        }

        /// <summary>
        /// Start the game. Check if the number of player is correct and reset the deck
        /// and all required settings.
        /// Expected state: WAITING_FOR_START
        /// Output state: ASKING_FOR_COLOR
        /// </summary>
        public void StartGame()
        {
            if (GameState != GameState.WAITING_FOR_START)
            {
                throw new GameStateUnauthorizedAction("Start the game", GameState);
            }

            // Shuffle card deck
            cardDeck.ShuffleDrawPile();

            // Set the "ace worse than two" rule
            Card.isAceWorseThanTwo = SpecialRules.IsAceWorseThanTwo;

            // Misc init
            lastRoundIsGiving = true;
            lastRoundNumberOfSips = 1;

            OnGameStarted?.Invoke();

            GameState = GameState.ASKING_FOR_COLOR;
            currentPlayerIdx = 0;
            OnPlayerTurn?.Invoke(CurrentPlayer, GameState);
        }

        /// <summary>
        /// Reset the game. Can be done at any moment of the game.
        /// Output state: NOT_STARTED
        /// </summary>
        public void ResetGame(bool keepPlayers)
        {
            cardDeck.ResetPiles();
            if (keepPlayers)
            {
                foreach (Player player in players)
                    player.Reset();
            }
            else
            {
                players.Clear();
                specialRules = new SpecialRules();
            }
            GameState = GameState.NOT_STARTED;
        }

        /// <summary>
        /// Set the choice of the current player for the round 1.
        /// Expected state: ASKING_FOR_COLOR
        /// </summary>
        /// <param name="choiceCardColor">The choice of the current player</param>
        public void SetChoiceCardColor(CardColor choiceCardColor)
        {
            if (GameState != GameState.ASKING_FOR_COLOR)
            {
                throw new GameStateUnauthorizedAction("Asking for color", GameState);
            }

            CurrentPlayer.ChosenCardColor = choiceCardColor;
        }

        /// <summary>
        /// Set the choice of the current player for the round 2.
        /// Expected state: ASKING_FOR_LESS_EQUAL_MORE
        /// </summary>
        /// <param name="choiceLessEqualMore">The choice of the current player</param>
        public void SetChoiceLessEqualMore(LessEqualMore choiceLessEqualMore)
        {
            if (GameState != GameState.ASKING_FOR_LESS_EQUAL_MORE)
            {
                throw new GameStateUnauthorizedAction("Asking for less/equal/more", GameState);
            }

            CurrentPlayer.ChosenLessEqualMore = choiceLessEqualMore;
        }

        /// <summary>
        /// Set the choice of the current player for the round 3.
        /// Expected state: ASKING_FOR_INTER_EQUAL_EXTER
        /// </summary>
        /// <param name="choiceInterEqualExter">The choice of the current player</param>
        public void SetChoiceInterEqualExter(InterEqualExter choiceInterEqualExter)
        {
            if (GameState != GameState.ASKING_FOR_INTER_EQUAL_EXTER)
            {
                throw new GameStateUnauthorizedAction("Asking for inter/equal/exter", GameState);
            }
            CurrentPlayer.ChosenInterEqualExter = choiceInterEqualExter;
        }

        /// <summary>
        /// Set the choice of the current player for the round 4.
        /// Expected state: ASKING_FOR_SUIT
        /// </summary>
        /// <param name="choiceCardSuit">The choice of the current player</param>
        public void SetChoiceCardSuit(CardSuit choiceCardSuit)
        {
            if (GameState != GameState.ASKING_FOR_SUIT)
            {
                throw new GameStateUnauthorizedAction("Asking for suit", GameState);
            }

            // Play Guillaume's Heart rule
            if (SpecialRules.IsGuillaumeHeart)
            {
                // If the rule is active and the name of the current player is "Guillaume"
                // automatically set Heart as the Card suit
                if (CurrentPlayer.Name == "Guillaume")
                {
                    CurrentPlayer.ChosenCardSuit = CardSuit.Heart;
                    return;
                }
            }

            CurrentPlayer.ChosenCardSuit = choiceCardSuit;
        }

        /// <summary>
        /// Draw next card and process the results for the current player (round 1 to 4) 
        /// or all the players (round 5).
        /// Expected state: ASKING_FOR_* or GIVING_OR_RECEIVING_DRINKS
        /// Output state: ASKING_FOR_* or GIVING_OR_RECEIVING_DRINKS or FINISHED
        /// </summary>
        public void RunNextDraw()
        {
            // Check state of the game
            if (GameState != GameState.ASKING_FOR_COLOR
                && GameState != GameState.ASKING_FOR_LESS_EQUAL_MORE
                && GameState != GameState.ASKING_FOR_INTER_EQUAL_EXTER
                && GameState != GameState.ASKING_FOR_SUIT
                && GameState != GameState.GIVING_OR_RECEIVING_DRINKS)
            {
                throw new GameStateUnauthorizedAction("Asking for suit", GameState);
            }

            // Draw a card
            Card card = cardDeck.DrawCard();
            OnCardDrawn?.Invoke(card);

            // Run the draw
            // - For round 1 to 4
            if (GameState == GameState.ASKING_FOR_COLOR
                || GameState == GameState.ASKING_FOR_LESS_EQUAL_MORE
                || GameState == GameState.ASKING_FOR_INTER_EQUAL_EXTER
                || GameState == GameState.ASKING_FOR_SUIT)
            {
                RunNextDrawRound1To4(card);
            }
            // - For round 5
            else if (GameState == GameState.GIVING_OR_RECEIVING_DRINKS)
            {
                RunNextDrawRound5(card);
            }

            // If there is no card left in the draw pile, the game is finished.
            if (cardDeck.DrawPileCount == 0)
            {
                GameState = GameState.FINISHED;
                OnGameFinished?.Invoke();
                return;
            }

            // Prepare next turn
            OnPlayerTurn(CurrentPlayer, GameState);
        }

        /// <summary>
        /// Run the draw for round 1 to 4. The card is set to the current player.
        /// The result is processed and sent to the UI.
        /// </summary>
        /// <param name="card">The card for this draw</param>
        void RunNextDrawRound1To4(Card card)
        {
            // Set the card to the current player
            CurrentPlayer.DrawCard(card, GameState);
            // The number of sips depends on the current state.
            // For convinient reasons, the integers values of GameState for round 1 to 4
            // are the number of the sips.
            int numberOfSips = (int)GameState;

            // Define whether the player must distribute or take sips
            bool giving;
            switch (GameState)
            {
                case GameState.ASKING_FOR_COLOR: giving = CurrentPlayer.HasChosenGoodColor(); break;
                case GameState.ASKING_FOR_LESS_EQUAL_MORE: giving = CurrentPlayer.HasChosenGoodLessEqualMore(); break;
                case GameState.ASKING_FOR_INTER_EQUAL_EXTER: giving = CurrentPlayer.HasChosenGoodInterEqualExter(); break;
                case GameState.ASKING_FOR_SUIT: giving = CurrentPlayer.HasChosenGoodCardSuit(); break;
                default: giving = false; break;
            }

            // Create the results
            List<PlayerDrawResult> drawResults = new List<PlayerDrawResult>();
            drawResults.Add(new PlayerDrawResult(CurrentPlayer, giving, 0, numberOfSips));
            // Send them
            OnDrawResults?.Invoke(drawResults);

            // Increment the current player idx for the next draw
            currentPlayerIdx++;
            // If the player idx equals the number of player, it means that the round is over.
            // In that case, reset the idx to 0 and increment the game state
            if (currentPlayerIdx == players.Count)
            {
                currentPlayerIdx = 0;
                GameState++;
            }
        }

        /// <summary>
        /// Run the draw for round 5. The result is processed and sent to the UI.
        /// </summary>
        /// <param name="card">The card for this draw</param>
        void RunNextDrawRound5(Card card)
        {
            List<PlayerDrawResult> drawResults = new List<PlayerDrawResult>();

            foreach (Player player in players)
            {
                int numberOfOccurences = player.HowManyOccurencesNumber(card);
                int numberOfSips = numberOfOccurences * lastRoundNumberOfSips;

                if (numberOfSips != 0)
                    drawResults.Add(new PlayerDrawResult(player, lastRoundIsGiving, numberOfOccurences, numberOfSips));
            }

            OnDrawResults?.Invoke(drawResults);

            // If the current round was "giving drinks", set the next round as "taking drinks"
            if (lastRoundIsGiving == true)
                lastRoundIsGiving = false;
            // If the current round was "taking drinks" and..
            else
            {
                // If next round is the death card, prepare for death...
                if (lastRoundNumberOfSips + 1 == numberOfSipsDeathCard)
                {
                    lastRoundIsGiving = false;
                    lastRoundNumberOfSips++;
                }
                // If the current round was the death card, reset number of sips
                else if (lastRoundNumberOfSips == numberOfSipsDeathCard)
                {
                    lastRoundIsGiving = true;
                    lastRoundNumberOfSips = 1;
                }
                // If the current round was a regular one, set the next round as "giving drinks" 
                // and increment the number of sip
                else
                {
                    lastRoundIsGiving = true;
                    lastRoundNumberOfSips++;
                }
            }
        }
    }
}
