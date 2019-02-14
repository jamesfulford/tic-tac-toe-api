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
        /// </summary>
        /// <param name="startBoard">The current state of the board.</param>
        /// <returns>A board state update.</returns>
        [HttpPost]
        [ProducesResponseType (typeof (Models.GameStateUpdate), (int) HttpStatusCode.OK)]
        [ProducesResponseType (typeof (Models.ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public ActionResult<Models.GameStateUpdate> ExecuteMove (Models.Board startBoard) {
            startBoard.CheckValidState ();

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
                // No-Op, basically
                return new ObjectResult (new Models.GameStateUpdate (
                    azurePlayerSymbol,
                    startBoard.Winner (),
                    null,
                    startBoard.gameBoard,
                    startBoard.WinningScenario ()
                ));
            }
        }
    }
}
