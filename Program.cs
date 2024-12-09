using ActressMas;

namespace MazeGeneration
{
    public class Program
    {
        private static ManualResetEvent mazeReadyEvent = new ManualResetEvent(false); // Event to signal maze completion

        public static void Main(string[] args)
        {
            // Create the ActressMas environment
            var env = new EnvironmentMas(0, 100);

            // Add the PlanetAgent to the environment
            var planetAgent = new PlanetAgent();
            env.Add(planetAgent, "planet");

            // Hook into the PlanetAgent's event to signal when the maze is ready
            planetAgent.MazeGenerated += OnMazeGenerated;

            // Run the PlanetForm in a separate thread to visualize the maze
            Thread guiThread = new Thread(() =>
            {
                var form = new PlanetForm();
                form.SetMaze(PlanetAgent.MazeGrid); // Set the maze grid from PlanetAgent
                Application.Run(form);
            });

            guiThread.SetApartmentState(ApartmentState.STA); // Required for Windows Forms
            guiThread.Start();

            // Start the ActressMas environment
            env.Start();

            // Wait for the maze to be generated
            mazeReadyEvent.WaitOne();

            // Display the maze grid in the console
            DisplayMazeInConsole(PlanetAgent.MazeGrid);
        }

        private static void OnMazeGenerated()
        {
            // Signal that the maze is ready
            mazeReadyEvent.Set();
        }

        private static void DisplayMazeInConsole(int[,] mazeGrid)
        {
            Console.WriteLine("Generated Maze:");
            for (int i = 0; i < mazeGrid.GetLength(0); i++)
            {
                for (int j = 0; j < mazeGrid.GetLength(1); j++)
                {
                    Console.Write(mazeGrid[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
