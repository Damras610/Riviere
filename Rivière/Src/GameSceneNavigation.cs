using Rivière.Screen;

namespace Rivière
{
    public class GameSceneNavigation
    {
        public IScreen CurrentScene { get; private set; }

        public void ShowSplashScreen() => ShowScreen(new SplashScreen());

        public void ShowLobbyScreen() => ShowScreen(new LobbyScreen());

        public void ShowIngameScreen() => ShowScreen(new IngameScreen());

        private void ShowScreen(IScreen screen)
        {
            if (CurrentScene != null)
                CurrentScene.Dispose();
            screen.Setup();
            Game.Instance.Desktop.Root = screen.SetupUI();
            CurrentScene = screen;
        }
    }
}
