namespace MazeGeneration
{
    public partial class PlanetForm : Form
    {
        private int[,] _mazeGrid;

        public PlanetForm()
        {
            InitializeComponent();
        }

        public void SetMaze(int[,] mazeGrid)
        {
            _mazeGrid = mazeGrid;
            Invalidate(); // Triggers a redraw of the form
        }

        private void PlanetForm_Paint(object sender, PaintEventArgs e)
        {
            if (_mazeGrid == null)
                return;

            DrawMaze(e.Graphics);
        }

        private void DrawMaze(Graphics g)
        {
            int cellSize = Math.Min(ClientSize.Width, ClientSize.Height) / _mazeGrid.GetLength(0);

            for (int x = 0; x < _mazeGrid.GetLength(0); x++)
            {
                for (int y = 0; y < _mazeGrid.GetLength(1); y++)
                {
                    if (_mazeGrid[x, y] == 1) // Wall
                    {
                        g.FillRectangle(Brushes.Black, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                    else // Path
                    {
                        g.FillRectangle(Brushes.White, x * cellSize, y * cellSize, cellSize, cellSize);
                    }

                    // Optional: Draw grid lines for better visualization
                    g.DrawRectangle(Pens.Gray, x * cellSize, y * cellSize, cellSize, cellSize);
                }
            }
        }
    }
}