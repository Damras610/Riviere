using System;
using System.Collections.Generic;
using System.Text;

namespace Rivière.BusinessLogic
{
    partial class GameLogic
    {
        // --------------- Callback for UI -------------------
        /// <summary>
        /// Invoked when the game is being prepared
        /// </summary>
        public Action OnGamePreparation;
        /// <summary>
        /// Invoked when a player is added to the list
        /// </summary>
        public Action<string> OnPlayerAdded;
        /// <summary>
        /// Invoked when a player is removed from the list
        /// </summary>
        public Action<string> OnPlayerRemoved;
        /// <summary>
        /// Invoked when the player are validated
        /// </summary>
        public Action<bool> OnPlayerValidated;
        /// <summary>
        /// Invoked when the game is started
        /// </summary>
        public Action OnGameStarted;
        /// <summary>
        /// Invoked when a player turn has began. This is called only for round 1 to 4
        /// </summary>
        public Action<Player, GameState> OnPlayerTurn;
        /// <summary>
        /// Invoked when a new card is drawn
        /// </summary>
        public Action<Card> OnCardDrawn;
        /// <summary>
        /// Invoked when the choice of the player has been compared to drawn card
        /// </summary>
        public Action<List<PlayerDrawResult>> OnDrawResults;
        /// <summary>
        /// Invoked when the game is finished
        /// </summary>
        public Action OnGameFinished;
        // Errors
        public Action<string> OnPlayerNameAlreadyTaken;
        public Action OnTooManyPlayers;
        public Action OnNotEnoughPlayers;


    }
}
