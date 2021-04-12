using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using Rivière.BusinessLogic;
using Rivière.Screen.UI;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rivière.Screen
{
    class LobbyScreen : IScreen
    {
        LobbyScreenUI ui;

        GameLogic GameLogic => Game.Instance.GameLogic;

        public void Setup()
        {
            GameLogic.OnPlayerAdded += AddPlayerLine;
            GameLogic.OnPlayerRemoved += RemovePlayerLine;
            GameLogic.OnPlayerNameAlreadyTaken += OnPlayerNameAlreadyTaken;
            GameLogic.OnNotEnoughPlayers += OnNotEnoughPlayers;
            GameLogic.OnTooManyPlayers += OnTooManyPlayers;
            GameLogic.OnPlayerValidated += OnPlayerValidated;
        }

        public Widget SetupUI()
        {
            ui = new LobbyScreenUI();

            // When the edit text is changed, toggle the "Add player" button.
            ui.playername_textbox.TextChanged += (s, e) =>
            {
                // Enable the button if ...
                ui.add_player_button.Enabled =
                    // the textbox is not empty ...
                    ui.playername_textbox.Text.Length > 0
                    // and the entered name is not already taken by another player.
                    && !GameLogic.players.Where(p => p.Name == ui.playername_textbox.Text).Any();
            };

            // When "Add player" / "Ok" button is clicked, add a player
            ui.add_player_button.Click += (s, e) => { GameLogic.AddPlayer(ui.playername_textbox.Text); };

            // When "Start the game" button is clicked, set the rules and validate the players
            ui.startgame_button.Click += (s, e) =>
            {
                // Set the rules
                GameLogic.SpecialRules = new SpecialRules()
                {
                    ArePlayersShuffled = ui.ruleshuffleplayer_checkbox.IsPressed,
                    IsGuillaumeHeart = ui.ruleguillaumeheart_checkbox.IsPressed,
                    IsAceWorseThanTwo = ui.ruleaceworst_checkbox.IsPressed,
                    AreThereJulienCheh = ui.rulejuliencheh_checkbox.IsPressed,
                    AreThereEricSubtitles = ui.ruleericsubtitles_checkbox.IsPressed
                };
                // Validate the players
                GameLogic.ValidatePlayer();
            };

            // Add player line for every player that are already in the game
            for (int i = 0; i < GameLogic.players.Count; i++)
                ui.AddPlayerLine(GameLogic.players[i].Name, i,
                                // OnRemove callback
                                (playerName) => GameLogic.RemovePlayer(playerName));

            // Set the special rule
            ui.ruleshuffleplayer_checkbox.IsPressed = GameLogic.SpecialRules.ArePlayersShuffled;
            ui.ruleguillaumeheart_checkbox.IsPressed = GameLogic.SpecialRules.IsGuillaumeHeart;
            ui.ruleaceworst_checkbox.IsPressed = GameLogic.SpecialRules.IsAceWorseThanTwo;
            ui.rulejuliencheh_checkbox.IsPressed = GameLogic.SpecialRules.AreThereJulienCheh;
            ui.ruleericsubtitles_checkbox.IsPressed = GameLogic.SpecialRules.AreThereEricSubtitles;

            return ui;
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && ui.add_player_button.Enabled == true)
            {
                GameLogic.AddPlayer(ui.playername_textbox.Text);
            }

            if (GameLogic.GameState == GameState.NOT_STARTED)
                GameLogic.PrepareGame();

            UpdateStartGameStatus();
        }

        public void Dispose()
        {
            GameLogic.OnPlayerAdded -= AddPlayerLine;
            GameLogic.OnPlayerRemoved -= RemovePlayerLine;
            GameLogic.OnPlayerNameAlreadyTaken -= OnPlayerNameAlreadyTaken;
            GameLogic.OnNotEnoughPlayers -= OnNotEnoughPlayers;
            GameLogic.OnTooManyPlayers -= OnTooManyPlayers;
            GameLogic.OnPlayerValidated -= OnPlayerValidated;
        }

        private void AddPlayerLine(string playerName) => 
            ui.AddPlayerLine(ui.playername_textbox.Text, GameLogic.players.Count,
                // OnRemove callback
                (playerName) => GameLogic.RemovePlayer(playerName));

        private void RemovePlayerLine(string playerName) => ui.RemovePlayerLine(playerName);


        private void UpdateStartGameStatus()
        {
            // Set visibility of "Start Game" button.
            ui.messagestartgame_label.Visible = GameLogic.players.Count < GameLogic.minPlayers || GameLogic.players.Count > GameLogic.maxPlayers;
            ui.startgame_button.Visible = !ui.messagestartgame_label.Visible;

            // Set the text of the message label
            if (GameLogic.players.Count < GameLogic.minPlayers)
            {
                ui.messagestartgame_label.Text = "Il faut encore inscrire " + (GameLogic.minPlayers - GameLogic.players.Count) + " joueurs.";
            }
            else if (GameLogic.players.Count > GameLogic.maxPlayers)
            {
                ui.messagestartgame_label.Text = "Il y a " + (GameLogic.players.Count - GameLogic.maxPlayers) + " joueurs en trop pour démarrer.";
            }
        }

        private void OnPlayerNameAlreadyTaken(string playerName)
        {

        }

        private void OnNotEnoughPlayers()
        {

        }

        private void OnTooManyPlayers()
        {

        }

        private void OnPlayerValidated(bool validated)
        {
            if (validated)
            {
                Game.Instance.GameSceneNavigation.ShowIngameScreen();
            }
        }
    }
}
