A single quadrant Marble Solitaire solver for CS 481.

# Assignment
2.0 Programming:
For the puzzle below code it up and use either the BFS or the DFS to do
this. For this version the end goal is a single marble anywhere (recall in
the full game the marble had to be in the middle). (Grad students code
up both DFS and BFS and compare and contrast the behavior of the two
algorithms)

# Result
This assignment was done with BFS, DFS, and IDS (Since they are such simple variations on the same algorithm). 
For this puzzle all solutions share the same depth so BFS does not provide any benefit as there are no infinite paths and it must traverse all layers of the tree before finding a solution. 
As a result it takes significantly longer to run BFS at an average of 31460ms over 10 trails when compared DFS at an average of 5ms over 100 trails.

Comparing the default DFS to IDS reveals that DFS is faster than IDS. This is likely due to one of the first few paths DFS takes leading to a goal on the default configuration of the tree, or perhaps Marble Solitaire has enough valid goal paths to not provide a significant benefit when using IDS.

