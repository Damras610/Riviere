using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Rivière.Screen.UI;
using System;
using System.Diagnostics;

namespace Rivière.Screen
{
    class SplashScreen : IScreen
    {
        SplashScreenUI ui;

        public double splashScreenRemainingTime;
        public double splashScreenTimeout = 5000;

        public void Setup()
        {
            splashScreenRemainingTime = splashScreenTimeout;
        }

        public void Update(GameTime gameTime)
        {
            splashScreenRemainingTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (splashScreenRemainingTime < 0)
                ui.start_button.Visible = true;
        }

        public Widget SetupUI()
        {
            ui = new SplashScreenUI();

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
