namespace MazeGeneration
{
    struct Point 
    {
        public int x;
        public int y;
    }

    public static class Utils
    {
        public static int Size = 24;
        public static Random RandNoGen = new Random();

        public static bool IsInsideGrid(int x, int y)
        {
            return x >= 0 && x < Size && y >= 0 && y < Size;
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
            PlanetAgent.MazeGrid[startX, startY] = 0;
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

            PlanetAgent.MazeGrid[0, 1] = 0;
            PlanetAgent.MazeGrid[n - 1, n - 2] = 0;
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

        public static void ParseMessage(string content, out string action, out string parameters)
        {
            string[] t = content.Split();

            action = t[0];

            parameters = "";

            if (t.Length > 1)
            {
                for (int i = 1; i < t.Length - 1; i++)
                    parameters += t[i] + " ";
                parameters += t[t.Length - 1];
            }
        }

        public Point parsePosition(string position) {
            string[] t = position.Split();
            Point p;
            p.x = int.Parse(t[0]);
            p.y = int.Parse(t[1]);
            return p;
        }

        public static string Str(object p1, object p2)
        {
            return string.Format("{0} {1}", p1, p2);
        }
    }
}