using System;

namespace TicTacToe.Models
{
    public class Board
    {
        public string[] gameBoard;

				public Boolean IsValidState()
				{
					return this.gameBoard.Length == 9;
				}
    }
}
