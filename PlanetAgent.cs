using ActressMas;

namespace MazeGeneration
{
    public class PlanetAgent : Agent
    {
        public static int[,] MazeGrid { get; private set; }
        public event Action MazeGenerated; // Event to signal maze completion

        public override void Setup()
        {
            Console.WriteLine("Starting maze generation...");

            // Initialize the maze grid: 1 for walls, 0 for paths
            MazeGrid = new int[Utils.Size, Utils.Size];
            for (int i = 0; i < Utils.Size; i++)
                for (int j = 0; j < Utils.Size; j++)
                    MazeGrid[i, j] = 1; // All walls initially

            GenerateMazeWithPrim();

            Console.WriteLine("Maze generation complete.");
            for (int i = 0; i < MazeGrid.GetLength(0); i++)
            {
                for (int j = 0; j < MazeGrid.GetLength(1); j++)
                {
                    Console.Write(MazeGrid[i, j] + " ");
                }
                Console.WriteLine();
            }

            MazeGenerated?.Invoke();
        }


        private void GenerateMazeWithPrim()
        {
            int n = Utils.Size;
            bool[,] visited = new bool[n, n];
            Random random = new Random();

            int[,] directions = { { -2, 0 }, { 2, 0 }, { 0, -2 }, { 0, 2 } };

            SortedSet<(int weight, int x, int y, int fromX, int fromY)> edges =
                new SortedSet<(int, int, int, int, int)>();

            int startX = random.Next(n);
            int startY = random.Next(n);
            MazeGrid[startX, startY] = 0;
            visited[startX, startY] = true;

            AddWallsToQueue(startX, startY, edges, visited);

            while (edges.Count > 0)
            {
                var (weight, x, y, fromX, fromY) = edges.Min;
                edges.Remove(edges.Min);
                if (visited[x, y]) continue;
                visited[x, y] = true;
                MazeGrid[x, y] = 0;
                MazeGrid[(x + fromX) / 2, (y + fromY) / 2] = 0;

                AddWallsToQueue(x, y, edges, visited);
            }

            MazeGrid[0, 1] = 0;
            MazeGrid[n - 1, n - 2] = 0;
        }

        private void AddWallsToQueue(int x, int y, SortedSet<(int weight, int x, int y, int fromX, int fromY)> edges, bool[,] visited)
        {
            Random random = new Random();

            // Directions for neighboring cells: [dx, dy]
            int[,] directions = { { -2, 0 }, { 2, 0 }, { 0, -2 }, { 0, 2 } };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int newX = x + directions[i, 0];
                int newY = y + directions[i, 1];

                // Check if the neighboring cell is within bounds and unvisited
                if (Utils.IsInsideGrid(newX, newY) && !visited[newX, newY])
                {
                    // Assign a random weight for the wall
                    int weight = random.Next(1, 101); // Random weight between 1 and 100

                    // Add the wall to the priority queue
                    edges.Add((weight, newX, newY, x, y));
                }
            }
        }


        private void AddWallsToList(int x, int y, Stack<(int, int)> wallList)
        {
            // Directions: [dx, dy]
            int[,] directions = { { -2, 0 }, { 2, 0 }, { 0, -2 }, { 0, 2 } };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int newX = x + directions[i, 0];
                int newY = y + directions[i, 1];
                if (Utils.IsInsideGrid(newX, newY) && MazeGrid[newX, newY] == 1)
                {
                    wallList.Push((newX, newY));
                }
            }
        }

        private bool CanCarve(int x, int y)
        {
            // Directions: [dx, dy]
            int[,] directions = { { -2, 0 }, { 2, 0 }, { 0, -2 }, { 0, 2 } };
            int pathCount = 0;

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int neighborX = x + directions[i, 0] / 2;
                int neighborY = y + directions[i, 1] / 2;
                if (Utils.IsInsideGrid(neighborX, neighborY) && MazeGrid[neighborX, neighborY] == 0)
                {
                    pathCount++;
                }
            }

            return pathCount == 1; // Only carve if it connects to exactly one path
        }
    }
}
