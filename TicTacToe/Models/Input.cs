using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe.Models
{
    public class Board
    {
        private static readonly string X = "X";
        private static readonly string O = "O";
        private static readonly string EMPTY = "?";

        public static readonly string INCONCLUSIVE = "inconclusive";
        private static readonly string TIE = "tie";

        private static readonly string[] Players = { X, O };
        private static readonly string[] Symbols = { X, O, EMPTY };

        private static readonly List<List<int>> winningIndexes = new List<List<int>> {
            // Horizontal wins
            new List<int> { 0, 1, 2 },
            new List<int> { 3, 4, 5 },
            new List<int> { 6, 7, 8 },
            // Vertical wins
            new List<int> { 0, 3, 6 },
            new List<int> { 1, 4, 7 },
            new List<int> { 2, 5, 8 },
            // Cross wins
            new List<int> { 0, 4, 8 },
            new List<int> { 2, 4, 6 },
        };

        public IEnumerable<string> gameBoard;

        public Board(IEnumerable<string> gameBoard)
        {
            this.gameBoard = gameBoard;
        }

        public Boolean IsValidState()
        {
            return this.gameBoard.Count() == 9
            && this.gameBoard.All(space => Symbols.Contains(space))
            && Math.Abs(
            this.gameBoard.Count(space => space == X)
            - this.gameBoard.Count(space => space == O)
            ) <= 1;
        }

        public string WhichPlayersTurn()
        {
            // The less common symbol is me.
            // if equally common, then I must have played first.
            // X is my favorite, so I always choose X when I go first.
            // (not that my code can have favorites or anything. :) )
            return this.gameBoard.Count(space => space == O)
                >= this.gameBoard.Count(space => space == X)
                ? X
                : O;
        }

        private IEnumerable<string> GetScenarioState(List<int> scenario)
        {
            return scenario.Select(i => this.gameBoard.ElementAt(i));
        }

        public List<int> WinningScenario()
        {
            foreach ( List<int> winScenario in winningIndexes )
            {
                IEnumerable<string> scenario = this.GetScenarioState(winScenario);

                // all things in list are equal
                string first = scenario.First();
                if (first != EMPTY)
                {
                    Boolean hasWinner = scenario.Skip(1).All(s => s == first);
                    if (hasWinner)
                    {
                        return winScenario;
                    }
                }
            }
            return null;
        }

        public string Winner()
        {
            List<int> winScenario = this.WinningScenario();
            if (winScenario != null) {
                return this.GetScenarioState(winScenario).First();
            }
            return this.gameBoard
                .All(s => s != EMPTY)
                ? TIE
                : INCONCLUSIVE;
        }

        public Board Play(int move, string playerSymbol)
        {
            return new Board(
                this.gameBoard.Select((s, i) =>
                    i == move
                        ? playerSymbol
                        : s
                )
            );
        }

        public int GetNextMove(string playerSymbol)
        {
            return 4;
        }
    }
}
