namespace ConsoleChess
{
    public static class Direction
    {
        public static (int x, int y) N = (0, 1), S = (0, -1), E = (1, 0), W = (-1, 0);
        public static (int x, int y) NE = (1, 1), NW = (-1, 1), SE = (1, -1), SW = (-1, -1);

        public static (int x, int y)[] Orthogonal = { N, S, E, W };
        public static (int x, int y)[] Diagonal = { NE, NW, SE, SW };
        public static (int x, int y)[] AllEight = { N, S, E, W, NE, NW, SE, SW };
    }
}
