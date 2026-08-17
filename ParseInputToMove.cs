using System;
using System.Collections.Generic;
namespace ConsoleChess
{
    public class MoveParser
    {
        public Move? Parse(string input, ChessBoard board)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            string[] parts = input.Trim().ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
            {
                if (parts[0].Length == 3)
                {
                    string pieceHint = parts[0][0].ToString();
                    string destination = parts[0].Substring(1, 2);
                    return ParsePieceToSquare(pieceHint, destination, board);
                }

                return ParseDestinationOnly(parts[0], board);
            }

            if (parts.Length == 2)
            {
                if (parts[0].Length == 2 && parts[1].Length == 2)
                {
                    return ParseCoordinates(parts[0], parts[1], board);
                }

                return ParsePieceToSquare(parts[0], parts[1], board);
            }

            if (parts.Length == 3 && parts[1] == "to")
            {
                return ParsePieceToSquare(parts[0], parts[2], board);
            }

            return null;
        }

        public Move? Parse(string fromStr, string toStr, ChessBoard board)
        {
            return ParseCoordinates(fromStr, toStr, board);
        }

        private Move? ParseCoordinates(string fromStr, string toStr, ChessBoard board)
        {
            if (string.IsNullOrWhiteSpace(fromStr) || string.IsNullOrWhiteSpace(toStr))
            {
                return null;
            }

            fromStr = fromStr.Trim().ToLowerInvariant();
            toStr = toStr.Trim().ToLowerInvariant();

            if (fromStr.Length != 2 || toStr.Length != 2)
            {
                return null;
            }

            // 'a' becomes index 0, 'b' becomes 1
            int fromX = fromStr[0] - 'a';
            int fromY = int.Parse(fromStr[1].ToString()) - 1;

            int toX = toStr[0] - 'a';
            int toY = int.Parse(toStr[1].ToString()) - 1;

            if (board.IsOffBoard(fromX, fromY) || board.IsOffBoard(toX, toY)) return null;

            Cell fromCell = board.GetCell(fromX, fromY);
            Cell toCell = board.GetCell(toX, toY);

            return new Move(fromCell, toCell);
        }

        private Move? ParseDestinationOnly(string toStr, ChessBoard board)
        {
            toStr = toStr.Trim().ToLowerInvariant();
            if (toStr.Length != 2)
            {
                return null;
            }

            int toX = toStr[0] - 'a';
            int toY = int.Parse(toStr[1].ToString()) - 1;

            if (board.IsOffBoard(toX, toY))
            {
                return null;
            }

            List<Move> matches = new List<Move>();
            List<Move> pawnMatches = new List<Move>();

            foreach (Piece piece in board.ActivePieces)
            {
                if (piece.Color != board.CurrentPlayer)
                {
                    continue;
                }

                foreach (Move candidate in piece.GetPseudoLegalMoves(board))
                {
                    if (candidate.To.X == toX && candidate.To.Y == toY && board.IsLegalMove(candidate))
                    {
                        matches.Add(candidate);
                        if (piece is Pawn)
                        {
                            pawnMatches.Add(candidate);
                        }
                    }
                }
            }

            if (pawnMatches.Count == 1)
            {
                return pawnMatches[0];
            }

            if (matches.Count == 1)
            {
                return matches[0];
            }

            return null;
        }

        private Move? ParsePieceToSquare(string pieceName, string toStr, ChessBoard board)
        {
            if (!TryGetPieceType(pieceName, out Type? pieceType))
            {
                return null;
            }

            toStr = toStr.Trim().ToLowerInvariant();
            if (toStr.Length != 2)
            {
                return null;
            }

            int toX = toStr[0] - 'a';
            int toY = int.Parse(toStr[1].ToString()) - 1;

            if (board.IsOffBoard(toX, toY))
            {
                return null;
            }

            List<Move> matches = new List<Move>();

            foreach (Piece piece in board.ActivePieces)
            {
                if (piece.Color != board.CurrentPlayer || piece.GetType() != pieceType)
                {
                    continue;
                }

                foreach (Move candidate in piece.GetPseudoLegalMoves(board))
                {
                    if (candidate.To.X == toX && candidate.To.Y == toY && board.IsLegalMove(candidate))
                    {
                        matches.Add(candidate);
                    }
                }
            }

            if (matches.Count == 1)
            {
                return matches[0];
            }

            return null;
        }

        private bool TryGetPieceType(string pieceName, out Type? pieceType)
        {
            switch (pieceName)
            {
                case "pawn":
                case "p":
                    pieceType = typeof(Pawn);
                    return true;
                case "rook":
                case "r":
                    pieceType = typeof(Rook);
                    return true;
                case "knight":
                case "n":
                    pieceType = typeof(Knight);
                    return true;
                case "bishop":
                case "b":
                    pieceType = typeof(Bishop);
                    return true;
                case "queen":
                case "q":
                    pieceType = typeof(Queen);
                    return true;
                case "king":
                case "k":
                    pieceType = typeof(King);
                    return true;
                default:
                    pieceType = null;
                    return false;
            }
        }
    }
}
