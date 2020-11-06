using System;

namespace Rivière
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = Game.Instance)
                game.Run();
        }
    }
}
