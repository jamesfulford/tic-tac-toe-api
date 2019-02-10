using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe.Models {
    /// <summary>
    /// Structure representing a given Tic Tac Toe board's state.
    /// </summary>
    public class Board {
        /// <summary>
        /// A position symbol indicating player X has placed in a position. Is also a player symbol. Is also a win-state indicating this player is the winner.
        /// </summary>
        private static readonly string X = "X";
        /// <summary>
        /// A position symbol indicating player O has placed in a position. Is also a player symbol. Is also a win-state indicating this player is the winner.
        /// </summary>
        private static readonly string O = "O";
        /// <summary>
        /// A position symbol indicating no player has placed in a position. Unlike X and O, EMPTY is not a player symbol.
        /// </summary>
        private static readonly string EMPTY = "?";

        /// <summary>
        /// A win-state indicating an unfinished game with no winner.
        /// </summary>
        public static readonly string INCONCLUSIVE = "inconclusive";
        /// <summary>
        /// A win-state indicating a finished game with no winner.
        /// </summary>
        private static readonly string TIE = "tie";

        private static readonly string[] Players = { X, O };
        private static readonly string[] Symbols = { X, O, EMPTY };
        /// <summary>
        /// WinningIndexes is a list of index lists which produce a win in Tic Tac Toe.
        /// For instance, the element [0, 1, 2] of WinningIndexes means that if a player plays in all of the top 3 positions of the board, they win.
        /// </summary>
        /// <value></value>
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
        /// <summary>
        /// A list of 9 position symbols ("X"|"O"|"?"), corresponding to the current state of a Tic Tac Toe board.
        /// Indices in this list correspond to a game board left-to-right, then top-to-bottom:
        ///   0 1 2
        ///   3 4 5
        ///   6 7 8
        /// </summary>
        public IEnumerable<string> gameBoard;

        /// <summary>
        /// A constructor for a Tic Tac Toe board state.
        /// </summary>
        /// <param name="gameBoard">A list of 9 player-symbols indicating the current board's state.</param>
        public Board (IEnumerable<string> gameBoard) {
            this.gameBoard = gameBoard;
        }

        /// <summary>
        /// Detects whether a board's state is valid.
        /// Valid boards have 9 symbols, all either "X"|"O"|"?",
        /// and the difference between "X" and "O" is no more than 1 play.
        /// </summary>
        /// <returns>Whether the current gameboard state is valid.</returns>
        public Boolean IsValidState () {
            return this.gameBoard.Count () == 9 &&
                this.gameBoard.All (space => Symbols.Contains (space)) &&
                Math.Abs (
                    this.gameBoard.Count (space => space == X) -
                    this.gameBoard.Count (space => space == O)
                ) <= 1;
        }

        /// <summary>
        /// Assuming the current state is waiting for the software to play,
        /// this method infers which player symbol the software should play as.
        /// Prefers "X".
        /// </summary>
        /// <returns>A player symbol ("X"|"O") believed to belong to this player.</returns>
        public string WhichPlayersTurn () {
            // The less common symbol is me.
            // if equally common, then I must have played first.
            // X is my favorite, so I always choose X when I go first.
            // (not that my code can have favorites or anything. :) )
            return this.gameBoard.Count (space => space == O) >=
                this.gameBoard.Count (space => space == X) ?
                X :
                O;
        }

        /// <summary>
        /// Given a list of indices, returns the list of position symbols filling those indices.
        /// </summary>
        /// <param name="scenario">A list of indices.</param>
        /// <returns>A list of position symbols ("X"|"O"|"?") corresponding to those indicies.</returns>
        private IEnumerable<string> GetScenarioState (List<int> scenario) {
            return scenario.Select (i => this.gameBoard.ElementAt (i));
        }

        /// <summary>
        /// Detects which scenario by which a player won the game, if there is a winner. Used for victory checking.
        /// </summary>
        /// <returns>A list of indices which are filled by the same player, indicating a win. If no player has won, returns null.</returns>
        public List<int> WinningScenario () {
            foreach (List<int> winScenario in winningIndexes) {
                IEnumerable<string> scenario = this.GetScenarioState (winScenario);

                // all things in list are equal
                string first = scenario.First ();
                if (first != EMPTY) {
                    Boolean hasWinner = scenario.Skip (1).All (s => s == first);
                    if (hasWinner) {
                        return winScenario;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the win-state of the current board.
        /// </summary>
        /// <returns>One of "X"|"O"|"tie"|"inconclusive".</returns>
        public string Winner () {
            List<int> winScenario = this.WinningScenario ();
            if (winScenario != null) {
                return this.GetScenarioState (winScenario).First ();
            }
            return this.gameBoard
                .All (s => s != EMPTY) ?
                TIE :
                INCONCLUSIVE;
        }

        /// <summary>
        /// Plays the move given.
        /// </summary>
        /// <param name="move">The index at which to place a symbol.</param>
        /// <param name="playerSymbol">The player symbol to place at the given index.</param>
        /// <returns>A new game state with the given position filled with the given symbol.</returns>
        public Board Play (int move, string playerSymbol) {
            return new Board (
                this.gameBoard.Select ((s, i) =>
                    i == move ?
                    playerSymbol :
                    s
                )
            );
        }

        /// <summary>
        /// The game heuristic which decides where to play the next move.
        /// </summary>
        /// <param name="playerSymbol">The symbol of the software player.</param>
        /// <returns>The index this software player intends to play.</returns>
        public int GetNextMove (string playerSymbol) {
            return 4;
        }
    }
}
