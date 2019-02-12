using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TicTacToe.Controllers {
    /// <summary>
    /// Plays a turn of Tic Tac Toe given the current board state.
    /// </summary>
    [Route ("api/executemove")]
    [ApiController]
    public class Controller : ControllerBase {
        /// <summary>
        /// Plays a turn of Tic Tac Toe given the current board state.
        /// Body is a JSON object with a startBoard attribute, whose value is a list of 9 strings representing the current game state. Acceptable strings are either a player symbol ("X"|"O") or "?" if empty.
        /// </summary>
        /// <returns>A new board state.</returns>
        [HttpPost]
        [ProducesResponseType (typeof (string), (int) HttpStatusCode.OK)]
        [ProducesResponseType (typeof (string), (int) HttpStatusCode.BadRequest)]
        public IActionResult Post () {
            // If I used [FromBody], then if "gameBoard" attribute is missing in JSON payload, then a 500 code is returned.
            string rawString = new StreamReader (Request.Body, System.Text.Encoding.UTF8).ReadToEnd ();
            Models.Board startBoard = null;
            try {
                startBoard = JsonConvert.DeserializeObject<Models.Board> (rawString);
                if (startBoard.gameBoard == null) {
                    throw new ArgumentException ();
                }
            } catch (Exception) {
                return BadRequest ();
            }
            if (!startBoard.IsValidState ()) {
                return BadRequest ();
            }

            string winner = startBoard.Winner ();
            string azurePlayerSymbol = startBoard.WhichPlayersTurn ();

            if (winner == Models.Board.INCONCLUSIVE) {
                // Game isn't over yet
                int move = startBoard.GetNextMove (azurePlayerSymbol);
                Models.Board nextBoard = startBoard.Play (move, azurePlayerSymbol);

                return new ObjectResult (new Models.GameStateUpdate (
                    azurePlayerSymbol,
                    nextBoard.Winner (),
                    move,
                    nextBoard.gameBoard,
                    nextBoard.WinningScenario ()
                ));
            } else {
                return new ObjectResult (new Models.GameStateUpdate (
                    azurePlayerSymbol,
                    startBoard.Winner ()
                ));
            }
        }
    }
}
