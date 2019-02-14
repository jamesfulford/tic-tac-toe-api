using System.Collections.Generic;

namespace TicTacToe.Models {
    /// <summary>
    /// Response given by controller containing details on the new state of the game.
    /// </summary>
    public class GameStateUpdate {
        /// <summary>
        /// Integer corresponding to index where last play took place.
        /// </summary>
        public int? move;

        /// <summary>
        /// Symbol azure assumes to play.
        /// </summary>
        public string azurePlayerSymbol;

        /// <summary>
        /// Win-state of game, one of "X"|"O"|"inconclusive"|"tie".
        /// </summary>
        public string winner;

        /// <summary>
        /// Array of positions used by winner to win, if game has been won.
        /// </summary>
        public IEnumerable<int> winPositions;

        /// <summary>
        /// Newest state of the gameBoard.
        /// </summary>
        public IEnumerable<string> gameBoard;

        /// <summary>
        /// Constructs a GameStateUpdate object.
        /// </summary>
        /// <param name="move">The move just played, if applicable.</param>
        /// <param name="azurePlayerSymbol">The symbol inferred to belong to the Azure API.</param>
        /// <param name="winner">The win-state of the game.</param>
        /// <param name="winPositions">The positions of the winning scenario, if applicable.</param>
        /// <param name="gameBoard">The new state of the board.</param>
        public GameStateUpdate (
            string azurePlayerSymbol,
            string winner,
            int? move,
            IEnumerable<string> gameBoard,
            IEnumerable<int> winPositions
        ) {
            this.move = move;
            this.azurePlayerSymbol = azurePlayerSymbol;
            this.winner = winner;
            this.winPositions = winPositions;
            this.gameBoard = gameBoard;
        }
    }
}
