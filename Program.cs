using System;
using System.Text;

namespace ConsoleChess
{
    class Program
    {
        static void Main(string[] args)
        {
            // displaying Unicode chess pieces
            Console.OutputEncoding = Encoding.UTF8;

            ChessBoard board = new ChessBoard();
            board.SetupStandardBoard(); // Initialize the 8x8 grid with pieces

            while (true)
            {
                Console.Clear();
                BoardRenderer.Render(board); // draws the board
                Console.ResetColor();
                Console.CursorVisible = true;

                if (board.IsCheckmate(board.CurrentPlayer))
                {
                    PlayerColor winner = board.CurrentPlayer == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
                    Console.WriteLine($"\nCheckmate! {winner} wins.");
                    break;
                }

                if (board.IsStalemate(board.CurrentPlayer))
                {
                    Console.WriteLine("\nStalemate! The game is a draw.");
                    break;
                }

                Console.WriteLine($"\n{board.CurrentPlayer}'s turn.");
                if (board.IsKingInCheck(board.CurrentPlayer))
                {
                    Console.WriteLine("Check!");
                }
                Console.Write("Move (e.g., e2 e4 or pawn to d4): ");
                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input) || input == "exit") break;

                try
                {
                    // Translates algebraic notion into a Move object
                    Move? move = board.ParseInputToMove(input);

                    if (move != null && board.IsLegalMove(move))
                    {
                        board.ExecuteMove(move);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Move!");
                        Console.ReadKey();
                    }
                }
                catch
                {
                    Console.WriteLine("Input error! Format: a2 a4 or pawn to d4.");
                    Console.ReadKey();
                }
            }
        }
    }
}
