using Rivière.BusinessLogic;
using System;

namespace Rivière
{
    class GameStateUnauthorizedAction : Exception
    {
        string action;
        GameState gameState;

        public GameStateUnauthorizedAction(string action, GameState gameState)
            : base("Unauthorized action \"" + action + "\" at game state " + gameState)
        {
            this.action = action;
            this.gameState = gameState;
        }
    }
}
