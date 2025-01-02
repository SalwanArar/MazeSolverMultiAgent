using ActressMas;

namespace MazeGeneration
{
    public class PlanetAgent : Agent
    {
        public static int[,] MazeGrid { get; private set; }
        public Dictionary<string, string> AgentPosition { get; set; }
        public enum AgentState { NotSpawned, Spawned, Dead };
        public Dictionary<string, AgentState> AgentStates { get; set; }

        private bool isExitFound = false;
        public event Action MazeGenerated; // Event to signal maze completion

        public override void Setup()
        {
            Console.WriteLine("Starting maze generation...");

            // Initialize the maze grid: 1 for walls, 0 for paths
            MazeGrid = new int[Utils.Size, Utils.Size];
            for (int i = 0; i < Utils.Size; i++)
                for (int j = 0; j < Utils.Size; j++)
                    MazeGrid[i, j] = 1; // All walls initially

            Utils.GenerateMazeWithPrim();

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

        public override void Act(Message message) 
        {
            Console.WriteLine("\t[{1} -> {0}]: {2}", this.Name, message.Sender, message.Content);
            
            string action; 
            string parameters;
            Utils.ParseMessage(message.Content, out action, out parameters);
        
            switch (action)
            {
                case "try_move":
                    HandleTryMove(message.Sender, parameters);
                    break;
                default:
                    break;
            }

            //need to update GUI
        }

        private void HandleTryMove(string sender, string position)
        {
            //position = "0 1"
            Point point = Utils.parsePosition(position);

            if (MazeGrid[point.x, point.y] == 1) //it's wall
            {
                Send(sender, "block");
                return;
            }

            // if it's another agent
            foreach (string agent in AgentPosition.Keys)
            {
                if (agent == sender)
                    continue;
                if (AgentPosition[agent] == position)
                {
                    // Besides just block, send also the explorer's name to comunicate the state.
                    Send(sender, Utils.Str("block", agent));
                    return;
                }
            }

            AgentPosition[sender] = position;

            if (position == exitPosition) // Cwhat is the exitPosition?
            {
                string messageType = isExitFound ? "exit" : "found";
                Send(sender, Utils.Str(messageType, position));

                if (!isExitFound)
                {
                    isExitFound = true;
                }

                // Mark the explorer as dead and remove it from the active list
                AgentStates[sender] = AgentState.Dead;
                AgentPosition.Remove(sender);

                Console.WriteLine("Remaining Explorers: {0}", AgentPosition.Count);

                // If no explorers remain, handle stopping the process
                if (ExplorerPositions.Count == 0)
                {
                    Console.WriteLine("{0}: Stopped", Name);
                    foreach (string agent in Environment.AllAgents())
                    {
                        Console.WriteLine("Remaining agent: {0}", agent);
                    }
                    this.Stop();
                }
                return;
            }            
            Send(sender, Utils.Str("move", position));
        }
    }
}
