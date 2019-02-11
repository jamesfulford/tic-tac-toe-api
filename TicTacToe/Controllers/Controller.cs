using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        /// Body is a JSON object with a startBoard attribute, whose value is a list of 9 strings representing the current game state. Acceptable strings are either a player symbol ("X"|"O") or "?" if empty.
        /// </summary>
        /// <returns>A new board state.</returns>
        [HttpPost]
        [ProducesResponseType(typeof (string), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof (string), (int) HttpStatusCode.BadRequest)]
        public IActionResult Post () {
            string rawString = new StreamReader(Request.Body, System.Text.Encoding.UTF8).ReadToEnd();
            Models.Board startBoard = null;
            try {
                startBoard = JsonConvert.DeserializeObject<Models.Board>(rawString);
                if (startBoard.gameBoard == null)
                {
                    throw new ArgumentException();
                }
            } catch (Exception) {
                return BadRequest();
            }

            if (!startBoard.IsValidState()) {
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
