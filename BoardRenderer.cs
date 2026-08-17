using System;
namespace ConsoleChess
{
    public static class BoardRenderer
    {
        public static void Render(ChessBoard board)
        {
            Console.Clear();
            const string indent = "  ";
            //print top labels
            Console.Write(indent);
            Console.Write("  ");
            for (char file = 'A'; file <= 'H'; file++)
            {
                Console.Write($"{file} ");
            }
            Console.WriteLine();
            //loop from rank(row) 8 down to rank 1
            for (int y = 7; y >= 0; y--)
            {
                //print left label(1-8), converted from index (0-7)
                Console.Write(indent);
                Console.Write($"{y + 1} ");
                for (int x = 0; x < 8; x++)
                {
                    Cell? cell = board.GetCell(x, y);
                    string square = cell == null || cell.IsEmpty ? ". " : $"{cell.Piece!.Symbol} ";
                    Console.Write(square);
                }
                //print right label(1-8)
                Console.WriteLine($"{y + 1}");
            }
            Console.Write(indent);
            Console.Write("  ");
            for (char file = 'A'; file <= 'H'; file++)
            {
                Console.Write($"{file} ");
            }
            Console.WriteLine();
        }
    }
}
