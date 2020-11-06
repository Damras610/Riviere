using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;
using Rivière.Screen.UI;
using System;

namespace Rivière.Screen
{
    class LobbyScreen : IScreen
    {
        LobbyScreenUI ui;

        public void Setup()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public Widget SetupUI()
        {
            ui = new LobbyScreenUI();

            ui.start_button.Click += (s, e) =>
            {
                Game.Instance.GameSceneNavigation.ShowLobbyScreen();
            };

            return ui;
        }

        public void Dispose()
        {

        }
    }
}
