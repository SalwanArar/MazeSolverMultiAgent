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
    }
}
