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
        public IActionResult Post([FromBody] Models.Board startBoard)
        {
					if (!startBoard.IsValidState()) {
              return BadRequest("Given state is invalid.");
					}
					return new ObjectResult(startBoard);
        }
    }
}
