using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// CS 481 AI
// mweger

namespace MarbleSolitaire
{
    /// <summary>
    /// Class to represent the game board of marble solitaire using a 2d array.
    /// </summary>
    class Board : GeneratesChildren<Board>
    {
        private SpaceState[,] m_Board; // Represents the game board. While the real board is a proper equilateral triangle, the board is represented as a right triangle residing with its corner in the bottom left cell.

        /// <summary>
        /// Creates a default Board instance with an empty slot at the center from the size param.
        /// </summary>
        /// <param name="size"></param> Size of the board.
        public Board(int size)
        {
            // For the equilateral triangle to have a proper center for the empty space it must be of the set of every other odd starting 1, 5, 9, ...
            // This set can be modeled by 4n - 3
            int n = (size + 3) / 4;
            if (size < 5 || size % 2 == 0 || size != 4 * n - 3)
                throw new ArgumentException("Board size must be an integer value of 5 or greater in the set of odd numbers of the set 4n - 3.");

            this.m_Board = new SpaceState[size, size];

            // Set up the board in a triangular fashion.
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (col <= row)
                    {
                        this.m_Board[row, col] = SpaceState.Marble;
                    }
                    // The matrix is going to have empty space so we need to indicate this
                    else
                    {
                        this.m_Board[row, col] = SpaceState.Void;
                    }
                }
            }

            // The empty marble should always reside in the center of the board
            int centerRow = size / 2;
            int centerCol = centerRow / 2;
            this.m_Board[centerRow, centerCol] = SpaceState.Empty;
        }

        /// <summary>
        /// Creates a board instance by performing the provided move on the given board state.
        /// </summary>
        /// <param name="previousBoard">The board state to start with.</param>
        /// <param name="move">The move to perform to get the new board state.</param>
        public Board(SpaceState[,] previousBoard, MarbleJump move)
        {
            this.m_Board = previousBoard;

            // Create the new board from the move which took place and the previous board
            this.m_Board[move.from[0], move.from[1]] = SpaceState.Empty;
            this.m_Board[move.over[0], move.over[1]] = SpaceState.Empty;
            this.m_Board[move.to[0],   move.to[1]]   = SpaceState.Marble;
        }

        /// <summary>
        /// Iterates over the board to find all moves which can be done from the current board state.
        /// </summary>
        /// <returns>A linked list of all potential moves</returns>
        private LinkedList<MarbleJump> GetMoves()
        {
            LinkedList<MarbleJump> moves = new LinkedList<MarbleJump>();

            for (int row = 0; row < this.m_Board.GetLength(0); row++)
            {
                for (int col = 0; col < this.m_Board.GetLength(1); col++)
                {
                    SpaceState state = this.m_Board[row, col];
                    if (state == SpaceState.Marble)
                    {
                        this.m_Board[row, col] = SpaceState.Marble;
                        AppendMoves(row, col, moves);
                    }
                    // No need to continue along rows that have hit void.
                    else if (state == SpaceState.Void)
                    {
                        break;
                    }
                }
            }

            return moves;
        }

        /// <summary>
        /// Appends all moves which can be made by the specified marble to the provided list.
        /// </summary>
        /// <param name="row">Row of the marble.</param>
        /// <param name="col">Column of the marble.</param>
        /// <param name="appendTo">The list to append moves to.</param>
        private void AppendMoves(int row, int col, LinkedList<MarbleJump> appendTo)
        {
            bool hitsTopBorder = row < 2;
            bool hitsBottomBorder = row > m_Board.GetLength(0) - 3;
            bool hitsLeftBorder = col < 2;
            bool hitsRightBorder = col > m_Board.GetLength(1) - 3;
            // Left
            // Only want to bother checking if we aren't at the edge of the board
            // col-1 checks one space to the left for a marble to hop. col-2 checks to see if the following space has an open slot
            if (!hitsLeftBorder && m_Board[row, col - 1] == SpaceState.Marble && m_Board[row, col - 2] == SpaceState.Empty)
            {
                appendTo.AddLast(new MarbleJump(JumpDirection.Left, new int[] { row, col }, new int[] { row, col - 1 }, new int[] { row, col - 2 }));
            }

            // Right
            // Again only checking if not on the edge.
            // Inverse of the leftwise movement. col+1 for marble check, col+2 for empty check.
            if (!hitsRightBorder && m_Board[row, col + 1] == SpaceState.Marble && m_Board[row, col + 2] == SpaceState.Empty)
            {
                appendTo.AddLast(new MarbleJump(JumpDirection.Right, new int[] { row, col }, new int[] { row, col + 1 }, new int[] { row, col + 2 }));
            }

            // Up right
            // While the board is a proper equilateral triangle, the board is represented as a right triangle residing with its corner in the bottom left cell.
            // So moving up on the matrix would be equivalent to a move up and to the right.
            // There's no need to check for the right border since finding void negates this move.
            if (!hitsTopBorder && m_Board[row - 1, col] == SpaceState.Marble && m_Board[row - 2, col] == SpaceState.Empty)
            {
                appendTo.AddLast(new MarbleJump(JumpDirection.UpRight, new int[] { row, col }, new int[] { row - 1, col }, new int[] { row - 2, col }));
            }

            // Down right
            // Now things are a bit trickier. To move down and right the column must be incremented.
            if (!hitsBottomBorder && m_Board[row + 1, col] == SpaceState.Marble && m_Board[row + 2, col] == SpaceState.Empty)
            {
                appendTo.AddLast(new MarbleJump(JumpDirection.DownRight, new int[] { row, col }, new int[] { row + 1, col }, new int[] { row + 2, col }));
            }

            // Up left
            // Much like moving down and right the column must be decremented. The left border must now also be minded.
            // This also means borders must be further checked
            if (!hitsTopBorder && !hitsLeftBorder && m_Board[row - 1, col - 1] == SpaceState.Marble && m_Board[row - 2, col - 2] == SpaceState.Empty)
            {
                appendTo.AddLast(new MarbleJump(JumpDirection.UpLeft, new int[] { row, col }, new int[] { row - 1, col - 1 }, new int[] { row - 2, col - 2 }));
            }

            // Down left
            // Similarily to moving up and to the right, down left is simply a movement along columns.
            if (!hitsBottomBorder && !hitsLeftBorder && m_Board[row + 1, col] == SpaceState.Marble && m_Board[row + 2, col] == SpaceState.Empty)
            {
                appendTo.AddLast(new MarbleJump(JumpDirection.DownLeft, new int[] { row, col }, new int[] { row + 1, col - 1 }, new int[] { row + 2, col - 2 }));
            }
        }

        /// <summary>
        /// Determines whether or not the board is solved in its current configuration.
        /// </summary>
        /// <returns>Whether or not the board is solved.</returns>
        public bool IsSolved()
        {
            int numMarbles = 0;

            for (int row = 0; row < this.m_Board.GetLength(0); row++)
            {
                for (int col = 0; col < this.m_Board.GetLength(1); col++)
                {
                    if (this.m_Board[row, col] == SpaceState.Marble)
                        numMarbles++;

                    if (numMarbles > 1)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Generates future board configurations from the current board and potential moves.
        /// </summary>
        /// <returns>A linked list of direct future board configurations.</returns>
        public LinkedList<Board> GenerateChildren()
        {
            LinkedList<Board> children = new LinkedList<Board>();
            LinkedList<MarbleJump> moves = this.GetMoves();

            foreach (MarbleJump move in moves)
            {
                children.AddLast(new Board((SpaceState[,]) this.m_Board.Clone(), move));
            }

            return children;
        }
        
        /// <summary>
        /// Creates a string representation of the board in a triangular fashion. X represents an empty space. O represents a marble space.
        /// </summary>
        /// <returns>The string representation of the board.</returns>
        public override String ToString()
        {
            String result = "";
            int size = this.m_Board.GetLength(0);
            int paddingLength = size - 1;

            for (int row = 0; row < size; row++)
            {
                for (int i = 0; i < size - row; i++)
                    result += " ";

                for (int col = 0; col < size; col++)
                {
                    switch (this.m_Board[row, col])
                    {
                        case SpaceState.Empty:
                            result += "X ";
                            break;
                        case SpaceState.Marble:
                            result += "O ";
                            break;
                        default:
                            break;
                    }
                }

                result += '\n';
            }

            return result;
        }

        /// <summary>
        /// States each position on the board can be in at any time.
        /// </summary>
        public enum SpaceState
        {
            Empty,
            Marble,
            Void // Unused spaces in the unused space in the 2d array.
        }
    }
}
