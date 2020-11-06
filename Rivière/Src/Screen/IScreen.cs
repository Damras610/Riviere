using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace Rivière.Screen
{
    public interface IScreen
    {
        void Setup();
        Widget SetupUI();
        void Update(GameTime gameTime);
        void Dispose();
    }
}
