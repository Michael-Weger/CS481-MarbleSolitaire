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
    /// Main class for a Marble Solitaire solver.
    /// </summary>
    class MarbleSolitaire
    {
        static void Main(string[] args)
        {
            try
            {

                int size = Convert.ToUInt16(args[0]); // Board size
                SolvingStrategy solvingStrategy;      // Strategy to traverse the state tree to solve the board

                switch (args[1].ToLower())
                {
                    case "bfs":
                    case "breadthfirstsearch":
                        solvingStrategy = SolvingStrategy.BreadthFirst;
                        break;
                    case "dfs":
                    case "depthfirstsearch":
                        solvingStrategy = SolvingStrategy.DepthFirst;
                        break;
                    case "ids":
                    case "iterativedeepening":
                        solvingStrategy = SolvingStrategy.IterativeDeepening;
                        break;
                    default:
                        throw new ArgumentException("Invalid solving strategy.");
                }

                Console.WriteLine("Solving for board size " + size + " with strategy of " + solvingStrategy + ".");


                // Run in data collection mode to get an average of n trials.
                if (args.Length > 2)
                {
                    int numTrials = Convert.ToUInt16(args[2]); // Number of trials to average.
                    long summedTimes = 0;

                    Console.WriteLine("Operating in data collection mode. Averaging over " + numTrials + " trials.");

                    for (int i = 0; i < numTrials; i++)
                    {
                        Solver solver = new Solver(size, solvingStrategy);
                        
                        long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                        
                        TreeNode<Board> result = solver.Solve();

                        long endTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                        long elapsedTime = endTime - startTime;
                        summedTimes += elapsedTime;
                        
                        Console.WriteLine("[Trial " + (i + 1) + "]: Elapsed time: " + elapsedTime + " milliseconds.");
                    }

                    long averageTime = summedTimes / numTrials;
                    Console.WriteLine("[Average]: Elapsed time: " + averageTime + " milliseconds.");
                }
                // Run in default mode for 1 run.
                else
                {
                    // Create the solver and store the start and end times to find the total time elapsed
                    Solver solver = new Solver(size, solvingStrategy);

                    long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    TreeNode<Board> result = solver.Solve();
                    long endTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                    if (result == null)
                    {
                        Console.WriteLine("No solution found.");
                    }
                    else
                    {
                        result.PrintAncestors();
                        Console.WriteLine("Final board state (Depth: " + result.Depth + ")");
                    }

                    Console.WriteLine("Elapsed time: " + (endTime - startTime) + " milliseconds.");
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Too few arguments specified. Specify board size and solving strategy.");
            }
            catch(FormatException ex)
            {
                Console.WriteLine("Please specify the board size or number of data collection iterations as UInt16.");
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }
    }
}
