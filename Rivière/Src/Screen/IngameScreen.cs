using Microsoft.Xna.Framework;
using MonoGame.Framework.Utilities;
using Myra.Graphics2D.UI;
using Rivière.BusinessLogic;
using Rivière.Screen.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Myra;
using Myra.Graphics2D.TextureAtlases;
using Rivière.Translations;
using Rivière.Utils;

namespace Rivière.Screen
{
    class IngameScreen : IScreen
    {
        IngameScreenUI ui;

        GameLogic GameLogic => Game.Instance.GameLogic;

        List<string> round5DrawNames = new List<string>();

        int round5Draw;

        public void Setup()
        {
            CreateRound5DrawNames();
            GameLogic.OnGameStarted += OnGameStarted;
            GameLogic.OnPlayerTurn += OnPlayerTurn;
            GameLogic.OnCardDrawn += OnCardDrawn;
            GameLogic.OnDrawResults += OnDrawResults;
            GameLogic.OnGameFinished += OnGameFinished;
            round5Draw = 0;
        }

        public Widget SetupUI()
        {
            ui = new IngameScreenUI();
            ui.score_grid.PopulateGrid(GameLogic.players.Select(p => p.Name).ToList(), round5DrawNames);
            ui.score_grid.movecolumnleft_button.Click += (s, e) => { ui.score_grid.MovableColumnsPosition--; };
            ui.score_grid.movecolumnright_button.Click += (s, e) => { ui.score_grid.MovableColumnsPosition++; };
            ui.remainingcardnumber_label.Text = GameLogic.cardDeck.DrawPileCount.ToString() + " cartes restantes";
            ui.drawcard_button.Click += (s, e) =>
            {
                
                StorePlayerChoice(GameLogic.CurrentPlayer, GameLogic.GameState);
                GameLogic.RunNextDraw();
            };
            ui.roundchoice_combobox.SelectedIndexChanged += (s, e) =>
            {
                ui.drawcard_button.Enabled = true;
            };
            return ui;
        }

        public void Update(GameTime gameTime)
        {
            if (GameLogic.GameState == GameState.NOT_STARTED)
            {
                Game.Instance.GameSceneNavigation.ShowLobbyScreen();
                return;
            }

            if (GameLogic.GameState == GameState.WAITING_FOR_START)
                GameLogic.StartGame();

            UpdateCardPiles();
            UpdateScoreGrid();
            UpdatePlayPanel();
        }

        public void Dispose()
        {
            GameLogic.OnGameStarted -= OnGameStarted;
            GameLogic.OnPlayerTurn -= OnPlayerTurn;
            GameLogic.OnCardDrawn -= OnCardDrawn;
            GameLogic.OnDrawResults -= OnDrawResults;
            GameLogic.OnGameFinished -= OnGameFinished;
        }

        private void UpdateCardPiles()
        {
            ui.drawpile_image.Visible = GameLogic.cardDeck.DrawPileCount > 0;
            ui.currentcard_image.Visible = GameLogic.cardDeck.DiscardPileCount > 0;

            int drawPileCount = GameLogic.cardDeck.DrawPileCount;
            if (drawPileCount == 0) ui.remainingcardnumber_label.Text = "Plus aucune carte";
            if (drawPileCount == 1) ui.remainingcardnumber_label.Text = "1 seule carte restante";
            if (drawPileCount >= 2) ui.remainingcardnumber_label.Text = drawPileCount + " cartes restantes";
        }

        private void UpdateScoreGrid()
        {
            // Update enability of the "move" buttons
            ui.score_grid.movecolumnleft_button.Enabled = ui.score_grid.MovableColumnsPosition > 0;
            ui.score_grid.movecolumnright_button.Enabled = ui.score_grid.MovableColumnsPosition + ui.score_grid.NumberOfMovableColumns < round5DrawNames.Count;

            // Color the cell of the current player
            foreach (Player player in GameLogic.players)
            {
                ui.score_grid.SetContentOfFixedColumns(GameLogic.players.IndexOf(player) + 1, 0, null, null, Color.White);
            }
            if (GameLogic.CurrentPlayer != null)
            {
                ui.score_grid.SetContentOfFixedColumns(GameLogic.players.IndexOf(GameLogic.CurrentPlayer) + 1, 0, null, null, Color.CornflowerBlue);
                ui.score_grid.SetContentOfFixedColumns(GameLogic.players.IndexOf(GameLogic.CurrentPlayer) + 1, (int)GameLogic.GameState, null, null, Color.CornflowerBlue);
            }
        }

        private void UpdatePlayPanel()
        {
            ui.roundname_label.Text = "----- " + GameLogic.GameState.Title() + " -----";
            ui.rounddesc_label.Text = GameLogic.GameState.Description();
            if (GameLogic.CurrentPlayer != null)
            {
                ui.currentplayername_label.Text = GameLogic.CurrentPlayer.Name;
            }

            // Activate the draw card button if the combobox is set to an item or it is invisible.
            ui.drawcard_button.Enabled = GameLogic.GameState != GameState.FINISHED
                && (ui.roundchoice_combobox.SelectedItem != null || !ui.roundchoice_combobox.Visible);
        }


        private void OnGameStarted()
        {

        }


        private void OnPlayerTurn(Player player, GameState gameState)
        {
            ui.roundchoice_combobox.Items.Clear();
            ui.drawcard_button.Enabled = false;


            if (gameState == GameState.ASKING_FOR_COLOR)
            {
                ui.roundchoice_combobox.Items.Add(new ListItem(CardColor.Red.Name(), null, CardColor.Red)); ;
                ui.roundchoice_combobox.Items.Add(new ListItem(CardColor.Black.Name(), null, CardColor.Black));
            }
            else if (gameState == GameState.ASKING_FOR_LESS_EQUAL_MORE)
            {
                ui.roundchoice_combobox.Items.Add(new ListItem(LessEqualMore.Less.Text(), null, LessEqualMore.Less));
                ui.roundchoice_combobox.Items.Add(new ListItem(LessEqualMore.Equal.Text(), null, LessEqualMore.Equal));
                ui.roundchoice_combobox.Items.Add(new ListItem(LessEqualMore.More.Text(), null, LessEqualMore.More));
            }
            else if (gameState == GameState.ASKING_FOR_INTER_EQUAL_EXTER)
            {
                ui.roundchoice_combobox.Items.Add(new ListItem(InterEqualExter.Inter.Text(), null, InterEqualExter.Inter));
                ui.roundchoice_combobox.Items.Add(new ListItem(InterEqualExter.Equal.Text(), null, InterEqualExter.Equal));
                ui.roundchoice_combobox.Items.Add(new ListItem(InterEqualExter.Exter.Text(), null, InterEqualExter.Exter));
            }
            else if (gameState == GameState.ASKING_FOR_SUIT)
            {
                // Play Guillaume rule
                if (GameLogic.SpecialRules.IsGuillaumeHeart && GameLogic.CurrentPlayer.Name == "Guillaume")
                {
                    ui.roundchoice_combobox.Items.Add(new ListItem(CardSuit.Heart.Name(), null, CardSuit.Heart));
                    ui.roundchoice_combobox.Items.Add(new ListItem(CardSuit.Heart.Name(), null, CardSuit.Heart));
                    ui.roundchoice_combobox.Items.Add(new ListItem(CardSuit.Heart.Name(), null, CardSuit.Heart));
                    ui.roundchoice_combobox.Items.Add(new ListItem(CardSuit.Heart.Name(), null, CardSuit.Heart));
                }
                else
                {
                    ui.roundchoice_combobox.Items.Add(new ListItem(CardSuit.Heart.Name(), null, CardSuit.Heart));
                    ui.roundchoice_combobox.Items.Add(new ListItem(CardSuit.Diamond.Name(), null, CardSuit.Diamond));
                    ui.roundchoice_combobox.Items.Add(new ListItem(CardSuit.Club.Name(), null, CardSuit.Club));
                    ui.roundchoice_combobox.Items.Add(new ListItem(CardSuit.Spade.Name(), null, CardSuit.Spade));
                }
            }
            else if (gameState == GameState.GIVING_OR_RECEIVING_DRINKS)
            {
                ui.playerturn_panel.Visible = false;
                ui.roundchoice_combobox.Visible = false;
            }
        }

        private void OnCardDrawn(Card card)
        {
            string cardRenderableResName = "images/cards/" + card.Number.ToString().ToLower() + "_" + card.Suit.ToString().ToLower() + ".png";
            ui.currentcard_image.Renderable = MyraEnvironment.DefaultAssetManager.Load<TextureRegion>(cardRenderableResName);

            if (GameLogic.GameState == GameState.ASKING_FOR_COLOR
                || GameLogic.GameState == GameState.ASKING_FOR_LESS_EQUAL_MORE
                || GameLogic.GameState == GameState.ASKING_FOR_INTER_EQUAL_EXTER
                || GameLogic.GameState == GameState.ASKING_FOR_SUIT)
            {
                ui.score_grid.SetContentOfFixedColumns(GameLogic.players.IndexOf(GameLogic.CurrentPlayer) + 1, (int)GameLogic.GameState,
                    card.Number.Name() + " " + card.Suit.Symbol(), card.Color.ToXNAColor(), null);
            }
            else
            {
                foreach (Player player in GameLogic.players)
                {
                    ui.score_grid.SetContentOfMovableColumns(GameLogic.players.IndexOf(player) + 1, round5Draw,
                        card.Number.Name() + " " + card.Suit.Symbol(), card.Color.ToXNAColor(), null);
                }

                // Center the grid on the current round (Move the position of the movable column accordingly)
                if (round5Draw + 1 >= ui.score_grid.MovableColumnsPosition + ui.score_grid.NumberOfMovableColumns)
                    ui.score_grid.MovableColumnsPosition = round5Draw - ui.score_grid.NumberOfMovableColumns + 2;
                if (round5Draw < ui.score_grid.MovableColumnsPosition)
                    ui.score_grid.MovableColumnsPosition = round5Draw;

            }

        }

        private void OnDrawResults(List<PlayerDrawResult> drawResults)
        {
            if (GameLogic.GameState == GameState.ASKING_FOR_COLOR
                || GameLogic.GameState == GameState.ASKING_FOR_LESS_EQUAL_MORE
                || GameLogic.GameState == GameState.ASKING_FOR_INTER_EQUAL_EXTER
                || GameLogic.GameState == GameState.ASKING_FOR_SUIT)
            {
                OnDrawResultsRound1To4(drawResults[0]);
            }
            else
            {
                OnDrawResultsRound5(drawResults);
            }
        }

        private void OnGameFinished()
        {
            GameFinishedDialog dialog = new GameFinishedDialog();
            dialog.ButtonOk.Click += (s, e) =>
            {
                GameLogic.ResetGame(dialog.keepplayers_checkbox.IsPressed);
            };
            dialog.ShowModal(Game.Instance.Desktop);
        }

        private void OnDrawResultsRound1To4(PlayerDrawResult playerDrawResult)
        {
            ui.score_grid.SetContentOfFixedColumns(GameLogic.players.IndexOf(GameLogic.CurrentPlayer) + 1, (int)GameLogic.GameState, null, null, (playerDrawResult.giving) ? Color.White : Color.Green);
        }

        private void OnDrawResultsRound5(List<PlayerDrawResult> drawResults)
        {
            foreach (PlayerDrawResult drawResult in drawResults)
            {
                Color? backgroundColor;
                switch (drawResult.numberOfOccurences)
                {
                    case 0: backgroundColor = null; break;
                    case 1: backgroundColor = Color.Green; break;
                    case 2: backgroundColor = Color.Yellow; break;
                    case 3: backgroundColor = Color.Orange; break;
                    default: backgroundColor = null; break;
                }

                ui.score_grid.SetContentOfMovableColumns(GameLogic.players.IndexOf(drawResult.player) + 1, round5Draw, null, null, backgroundColor);
            }
            round5Draw++;
        }

        private void CreateRound5DrawNames()
        {
            // The number of card used for the round 5 is equal to the total number of cards - 4 * numberOfPlayer.
            // Indeed, each player draw 4 cards during the first four rounds
            int numberOfCardsForRound5 = GameLogic.cardDeck.DrawPileCount - 4 * GameLogic.players.Count;

            bool giving = true;
            int numberOfSip = 1;

            for (int i = 0; i < numberOfCardsForRound5; i++)
            {
                string drawName;
                if (giving)
                    drawName = "Donne " + numberOfSip;
                else if (numberOfSip == GameLogic.numberOfSipsDeathCard)
                    drawName = "La mort";
                else
                    drawName = "Boit " + numberOfSip;
                round5DrawNames.Add(drawName);

                /* COPY OF THE GAME LOGIC ROUND 5 LOGIC */
                // If the current round was "giving drinks", set the next round as "taking drinks"
                if (giving == true)
                    giving = false;
                // If the current round was "taking drinks" and..
                else
                {
                    // If next round is the death card, prepare for death...
                    if (numberOfSip + 1 == GameLogic.numberOfSipsDeathCard)
                    {
                        giving = false;
                        numberOfSip++;
                    }
                    // If the current round was the death card, reset number of sips
                    else if (numberOfSip == GameLogic.numberOfSipsDeathCard)
                    {
                        giving = true;
                        numberOfSip = 1;
                    }
                    // If the current round was a regular one, set the next round as "giving drinks" 
                    // and increment the number of sip
                    else
                    {
                        giving = true;
                        numberOfSip++;
                    }
                }
                /* END OF COPY OF THE GAME LOGIC ROUND 5 LOGIC */
            }
        }

        private void StorePlayerChoice(Player player, GameState gameState)
        {
            if (gameState == GameState.ASKING_FOR_COLOR)
            {
                GameLogic.SetChoiceCardColor((CardColor)ui.roundchoice_combobox.SelectedItem.Tag);
            }
            else if (gameState == GameState.ASKING_FOR_LESS_EQUAL_MORE)
            {
                GameLogic.SetChoiceLessEqualMore((LessEqualMore)ui.roundchoice_combobox.SelectedItem.Tag);
            }
            else if (gameState == GameState.ASKING_FOR_INTER_EQUAL_EXTER)
            {
                GameLogic.SetChoiceInterEqualExter((InterEqualExter)ui.roundchoice_combobox.SelectedItem.Tag);
            }
            else if (gameState == GameState.ASKING_FOR_SUIT)
            {
                GameLogic.SetChoiceCardSuit((CardSuit)ui.roundchoice_combobox.SelectedItem.Tag);
            }
        }
    }
}
