using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TicTacToe.Controllers
{
    /// <summary>
    /// Plays a turn of Tic Tac Toe given the current board state.
    /// </summary>
    [Route("api/executemove")]
    [ApiController]
    public class Controller : ControllerBase
    {
        /// <summary>
        /// Plays a turn of Tic Tac Toe given the current board state.
        /// </summary>
        /// <param name="startBoard">Required. List of strings representing the current game state. Acceptable strings are either a player symbol ("X"|"O") or "?" if empty.</param>
        /// <returns>A new board state.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.BadRequest)]
        // If body is not expected JSON schema, should give 400.
        public IActionResult Post([FromBody] Models.Board startBoard)
        {
            if (!startBoard.IsValidState())
            {
                // TODO(optional): give better explanations
                return BadRequest("Given state is invalid.");
            }

            string winner = startBoard.Winner();
            string azurePlayerSymbol = startBoard.WhichPlayersTurn();

            int move;
            Models.Board nextBoard = startBoard;
            if (winner == Models.Board.INCONCLUSIVE) {
                // Game isn't over yet
                move = startBoard.GetNextMove(azurePlayerSymbol);
                nextBoard = startBoard.Play(move, azurePlayerSymbol);
            }

            // return {
            //     move: move,
            //     azurePlayerSymbol: azurePlayerSymbol,
            //     winner: move == null
            //         ? winner
            //         : nextBoard.Winner(),  // Small performance improvement
            //     winPositions: nextBoard.WinningScenario(),
            //     gameBoard: nextBoard,
            // }
            return new ObjectResult(nextBoard);
            // TODO: return a proper object
        }
    }
}
