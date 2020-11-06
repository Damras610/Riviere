using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using Rivière.BusinessLogic;
using System.IO;
using XNAssets;
using XNAssets.Utility;

namespace Rivière
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private static Game instance;
        public static Game Instance
        {
            get
            {
                if (instance == null)
                    instance = new Game();
                return instance;
            }
        }

        public Desktop Desktop;
        public GameSceneNavigation GameSceneNavigation;
        public GameLogic GameLogic;

        GraphicsDeviceManager GraphicsDeviceManager;
        AssetManager assetManager;

        const int windowWidth = 1600;
        const int windowHeight = 900;
        const bool windowFullscreen = false;

        Game()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);

            // Windows and misc settings
            IsMouseVisible = true;
            GraphicsDeviceManager.PreferredBackBufferWidth = windowWidth;
            GraphicsDeviceManager.PreferredBackBufferHeight = windowHeight;
            GraphicsDeviceManager.IsFullScreen = windowFullscreen;
        }

        protected override void Initialize()
        {
            // Load the asset manager
            FileAssetResolver assetResolver = new FileAssetResolver(PathUtils.ExecutingAssemblyDirectory);
            assetManager = new AssetManager(GraphicsDevice, assetResolver);

            GameSceneNavigation = new GameSceneNavigation();
            GameLogic = new GameLogic();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Setup Myra
            MyraEnvironment.Game = this;

            // Load stylesheet
            Stylesheet.Current = assetManager.Load<Stylesheet>("stylesheets/riviere_ui_skin.xml");

            Desktop = new Desktop();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (GameSceneNavigation.CurrentScene == null)
                GameSceneNavigation.ShowSplashScreen();

            GameSceneNavigation.CurrentScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Desktop.Render();
            base.Draw(gameTime);
        }
    }
}
