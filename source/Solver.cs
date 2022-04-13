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
    /// Solves a default Marble Solitaire board.
    /// </summary>
    class Solver
    {
        private SolvingStrategy m_Strategy; // Strategy by which to solve.
        private TreeNode<Board> m_Head; // Head node.

        /// <summary>
        /// Creates a solver for the specified board size and strategy.
        /// </summary>
        /// <param name="boardSize">Size of the default board.</param>
        /// <param name="strategy">Strategy by which to solve.</param>
        public Solver(int boardSize, SolvingStrategy strategy)
        {
            this.m_Head = new TreeNode<Board>(new Board(boardSize));
            this.m_Strategy = strategy;
        }

        /// <summary>
        /// Solves the board by the solver's strategy.
        /// </summary>
        /// <returns>The final board configuration.</returns>
        public TreeNode<Board> Solve()
        {
            switch (this.m_Strategy)
            {
                case SolvingStrategy.BreadthFirst:
                    return this.BreadthFirstSearch();
                case SolvingStrategy.DepthFirst:
                    return this.DepthFirstSearch();
                case SolvingStrategy.IterativeDeepening:
                    return this.IterativeDeepeningSearch(1);
            }

            throw new ArgumentException("Invalid solving strategy");
        }

        /// <summary>
        /// Solves the board by a breadth first search.
        /// </summary>
        /// <returns>The final board configuration.</returns>
        private TreeNode<Board> BreadthFirstSearch()
        {
            LinkedList<TreeNode<Board>> open   = new LinkedList<TreeNode<Board>>();
            LinkedList<TreeNode<Board>> closed = new LinkedList<TreeNode<Board>>();
            open.AddFirst(this.m_Head);

            while (open.Count != 0)
            {
                TreeNode<Board> leftmost = open.First();
                open.RemoveFirst();

                if (leftmost.Data.IsSolved())
                {
                    return leftmost;
                }
                else
                {
                    LinkedList<TreeNode<Board>> children = leftmost.Children;
                    closed.AddFirst(leftmost);

                    foreach (TreeNode<Board> child in children)
                    {
                        if (!open.Contains(child) && !closed.Contains(child))
                        {
                            open.AddLast(child);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Solves the board by a depth first search.
        /// </summary>
        /// <returns>The final board configuration.</returns>
        private TreeNode<Board> DepthFirstSearch()
        {
            LinkedList<TreeNode<Board>> open = new LinkedList<TreeNode<Board>>();
            LinkedList<TreeNode<Board>> closed = new LinkedList<TreeNode<Board>>();
            open.AddFirst(this.m_Head);

            while (open.Count != 0)
            {
                TreeNode<Board> leftmost = open.First();
                open.RemoveFirst();

                if (leftmost.Data.IsSolved())
                {
                    return leftmost;
                }
                else
                {
                    LinkedList<TreeNode<Board>> children = leftmost.Children;
                    closed.AddFirst(leftmost);

                    foreach (TreeNode<Board> child in children)
                    {
                        if (!open.Contains(child) && !closed.Contains(child))
                        {
                            open.AddFirst(child);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Solves the board by a depth first search with iterative deepening.
        /// </summary>
        /// <returns>The final board configuration.</returns>
        private TreeNode<Board> IterativeDeepeningSearch(int maxDepth)
        {
            LinkedList<TreeNode<Board>> open = new LinkedList<TreeNode<Board>>();
            LinkedList<TreeNode<Board>> closed = new LinkedList<TreeNode<Board>>();
            open.AddFirst(this.m_Head);

            while (open.Count != 0)
            {
                TreeNode<Board> leftmost = open.First();
                open.RemoveFirst();

                if (leftmost.Data.IsSolved())
                {
                    return leftmost;
                }
                else if (leftmost.Depth > maxDepth)
                {
                    return IterativeDeepeningSearch(maxDepth + 1);
                }
                else
                {
                    LinkedList<TreeNode<Board>> children = leftmost.Children;
                    closed.AddFirst(leftmost);

                    foreach (TreeNode<Board> child in children)
                    {
                        if (!open.Contains(child) && !closed.Contains(child))
                        {
                            open.AddFirst(child);
                        }
                    }
                }
            }

            return null;
        }
    }

    /// <summary>
    /// The strategies by which the solver can solve a board.
    /// </summary>
    enum SolvingStrategy
    {
        BreadthFirst,
        DepthFirst,
        IterativeDeepening
    }
}
