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
        /// Throws an exception if board is in an invalid state.
        /// </summary>
        /// <returns>Whether the current gameboard state is valid.</returns>
        /// <remarks>
        /// Valid boards have 9 symbols, all either "X"|"O"|"?",
        /// and the difference between "X" and "O" is no more than 1 play.
        /// </remarks>
        public void CheckValidState () {
            if (this.gameBoard.Count () != 9) {
                throw new ArgumentException ("\"gameBoard\" must have exactly 9 elements.");
            }
            if (!this.gameBoard.All (space => Symbols.Contains (space))) {
                throw new ArgumentException ("All elements of \"gameBoard\" must be one of \"X\", \"O\", or \"?\".");
            }
            int xminuso = this.gameBoard.Count (space => space == X) -
                this.gameBoard.Count (space => space == O);
            if (Math.Abs (xminuso) > 1) {
                throw new ArgumentException ($"Cannot skip turns. {(xminuso > 0 ? "X" : "O")} has {Math.Abs(xminuso)} more plays than {(xminuso < 0 ? "X" : "O")}, indicating skipping of the latter's turn(s).");
            }
        }

        /// <summary>
        /// Infers which player symbol the software should play as.
        /// </summary>
        /// <returns>A player symbol ("X"|"O") believed to belong to this player.</returns>
        /// <remarks>
        /// Prefers "X" in cases where it cannot decide.
        /// </remarks>
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
        private IEnumerable<string> GetScenarioState (IEnumerable<int> scenario) {
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

        // My strategy:
        // 1. Win
        // 2. Block an immediate win
        // 3. Build a two-way threat (2 scenarios will upgrade to level 2)
        //      (think of a two-way threat as a checkmate)
        // 4. Block a two-way threat
        // 5. Build a 1-way threat (1 scenario will upgrade to level 2)
        // 6. Place in a generically strategic position.

        private static readonly List<int> winWeights = new List<int> {
            1, // if not close to winning, not a high priority (beginning of game, middle=4)
            5, // if will get close to winning, worth considering.
            // Two-way threat scores a 10.
            1000 // if will win, then place the token already!
        };
        private static readonly List<int> loseWeights = new List<int> {
            0, // enemy gives no reason to place here.
            4, // block the enemy
            // Two-way block scores an 8 (higher than building a one-way threat, lower than building a two-way threat)
            100 // if will lose, then prioritize blocking (lower than winning, of course)
        };

        /// <summary>
        /// The game heuristic which decides where to play the next move.
        /// </summary>
        /// <param name="playerSymbol">The symbol of the software player.</param>
        /// <returns>The index this software player intends to play.</returns>
        public int GetNextMove (string playerSymbol) {
            string otherPlayer = playerSymbol == X ? O : X;

            // Plan: whichever index has the highest score gets played.
            List<int> scores = new List<int> {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
            };

            // Give points based on progress toward my player winning:
            IEnumerable<IEnumerable<int>> winableScenarios = Board.winningIndexes.Where (scen =>
                // keep scenarios containing no enemy placements (can be won)
                this.GetScenarioState (scen).All (s => s != otherPlayer)
            );
            foreach (List<int> scenario in winableScenarios) {
                IEnumerable<string> state = this.GetScenarioState (scenario);
                int count = state.Count (s => s == playerSymbol);
                foreach (int index in scenario) {
                    if (((List<string>) this.gameBoard) [index] == EMPTY) {
                        scores[index] += winWeights[count];
                    }
                }
            }

            // Give points based on enemy progress toward winning
            IEnumerable<IEnumerable<int>> losableScenarios = Board.winningIndexes.Where (scen =>
                // keep scenarios containing no enemy placements (can be won)
                this.GetScenarioState (scen).All (s => s != playerSymbol)
            );
            foreach (List<int> scenario in losableScenarios) {
                IEnumerable<string> state = this.GetScenarioState (scenario);
                int count = state.Count (s => s == otherPlayer);
                foreach (int index in scenario) {
                    if (((List<string>) this.gameBoard) [index] == EMPTY) {
                        scores[index] += loseWeights[count];
                    }
                }
            }

            // Find highest-valued index
            return Enumerable.Range (0, scores.Count ())
                .Where (i => this.gameBoard.ElementAt (i) == EMPTY)
                .Aggregate ((max, i) => scores[max] > scores[i] ? max : i);
        }
    }
}
