using System;
using System.Collections.Generic;
using System.Linq;

namespace Rivière.BusinessLogic
{
    public class GameLogic
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

        // --------------- Callback for UI -------------------
        // Game
        /// <summary>
        /// OnPlayerResult is invoked at each draw.
        /// </summary>
        public Action<List<PlayerDrawResult>> OnDrawResults;
        public Action OnGameFinished;
        // Errors
        public Action OnPlayerNameAlreadyTaken;
        public Action OnTooManyPlayers;
        public Action OnNotEnoughPlayers;

        /// <summary>
        /// The current list of players
        /// </summary>
        public readonly List<Player> players = new List<Player>();
        /// <summary>
        /// The card deck
        /// </summary>
        public readonly CardDeck cardDeck = new CardDeck();

        /// <summary>
        /// The maximum number of players to start a game.
        /// </summary>
        const int maxPlayers = 10;
        /// <summary>
        /// The minimum number of players to start a game.
        /// </summary>
        const int minPlayers = 4;
        /// <summary>
        /// The number of sips during the death card. It is important because once we reach
        /// the death card, the number of sips during last round goes back to 1, until the 
        /// deck is empty.
        /// </summary>
        int numberOfSipsDeathCard = 6;

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
                OnPlayerNameAlreadyTaken();
                return;
            }

            players.Add(new Player(playerName));
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

            players.RemoveAll(p => p.Name == playerName);
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

            GameState = GameState.WAITING_FOR_START;
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

            if (players.Count() > maxPlayers)
            {
                OnTooManyPlayers();
                return;
            }
            else if (players.Count < minPlayers)
            {
                OnNotEnoughPlayers();
                return;
            }

            cardDeck.ShuffleDrawPile();

            currentPlayerIdx = 0;
            lastRoundIsGiving = true;
            lastRoundNumberOfSips = 1;
            GameState = GameState.ASKING_FOR_COLOR;
        }

        /// <summary>
        /// Reset the game. Can be done at any moment of the game.
        /// Output state: NOT_STARTED
        /// </summary>
        public void ResetGame()
        {
            cardDeck.ResetPiles();
            players.Clear();
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

            CurrentPlayer.SetChoiceCardColor(choiceCardColor);
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

            CurrentPlayer.SetChoiceLessEqualMore(choiceLessEqualMore);
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
            CurrentPlayer.SetChoiceInterEqualExter(choiceInterEqualExter);
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

            CurrentPlayer.SetChoiceCardSuit(choiceCardSuit);
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

            // If there is no card left in the draw pile, the game is finished.
            if (cardDeck.DrawPileCount() == 0)
            {
                GameState = GameState.FINISHED;
                OnGameFinished();
                return;
            }

            // Draw a card
            Card card = cardDeck.DrawCard();


            // For round 1 to 4
            if (GameState == GameState.ASKING_FOR_COLOR
                || GameState == GameState.ASKING_FOR_LESS_EQUAL_MORE
                || GameState == GameState.ASKING_FOR_INTER_EQUAL_EXTER
                || GameState == GameState.ASKING_FOR_SUIT)
            {
                RunNextDrawRound1To4(card);
            }

            // For round 5
            else if (GameState == GameState.GIVING_OR_RECEIVING_DRINKS)
            {
                RunNextDrawRound5(card);
            }
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
            drawResults.Add(new PlayerDrawResult(CurrentPlayer, giving, numberOfSips));
            // Send them
            OnDrawResults(drawResults);

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
                int numberOfSips = player.HowManyOccurencesNumber(card) * lastRoundNumberOfSips;

                if (numberOfSips != 0)
                    drawResults.Add(new PlayerDrawResult(player, lastRoundIsGiving, numberOfSips));
            }

            OnDrawResults(drawResults);

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
                    lastRoundIsGiving = false;
                    lastRoundNumberOfSips = 1;
                }
                // If the current round was a regular one, set the next round as "giving drinks" 
                // and increment the number of sip
                else
                {
                    lastRoundIsGiving = false;
                    lastRoundNumberOfSips++;

                }

            }
        }
    }
}
