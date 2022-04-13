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
    /// Represents a move taken in Marble Solitaire.
    /// </summary>
    public class MarbleJump
    {
        public readonly JumpDirection direction; // Direction jumped in.
        public readonly int[] from; // Original position of the marble.
        public readonly int[] over; // The space jumped over.
        public readonly int[] to; // The new position of the marble.

        /// <summary>
        /// Creates a new move from the direction, previous, jumped, and new positions.
        /// </summary>
        /// <param name="direction">Direction moved in.</param>
        /// <param name="from">Original position.</param>
        /// <param name="over">Intermediate position jumped over.</param>
        /// <param name="to">Final position.</param>
        public MarbleJump(JumpDirection direction, int[] from, int[] over, int[] to)
        {
            this.direction = direction;
            this.from = from;
            this.over = over;
            this.to = to;
        }

        /// <summary>
        /// Returns a string representation of this move.
        /// </summary>
        /// <returns>A string representation of this move.</returns>
        public override String ToString()
        {
            return direction + ", {From: " + from[0] + ", " + from[1] + "}, {Over: " + over[0] + ", " + over[1] + "}, To: " + to[0] + ", " + to[1] + "}";
        }
    }

    /// <summary>
    /// Enumeration of directions a marble can move in.
    /// </summary>
    public enum JumpDirection
    {
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }
}
