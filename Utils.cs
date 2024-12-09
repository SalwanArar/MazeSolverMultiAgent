namespace MazeGeneration
{
    public static class Utils
    {
        public static int Size = 24;
        public static Random RandNoGen = new Random();

        public static bool IsInsideGrid(int x, int y)
        {
            return x >= 0 && x < Size && y >= 0 && y < Size;
        }
    }
}