using System;
using System.Collections.Generic;
namespace ConsoleChess
{
    public class Pawn : Piece
    {
        public override char Symbol => Color == PlayerColor.White ? '♙' : '♟';

        public Pawn(PlayerColor color) : base(color) { }

        internal override List<Move> GetPseudoLegalMoves(ChessBoard board)
        {
            var moves = new List<Move>();

            // Pawn direction: white moves up (y+1), black moves down (y-1)
            int direction = (Color == PlayerColor.White) ? 1 : -1;

            // Forward one square
            int oneSquareY = ParentCell.Y + direction;
            if (!board.IsOffBoard(ParentCell.X, oneSquareY))
            {
                Cell oneSquareCell = board.GetCell(ParentCell.X, oneSquareY);
                if (oneSquareCell.IsEmpty)
                {
                    moves.Add(new Move(ParentCell, oneSquareCell));

                    // Forward two squares on first move
                    if (!HasMoved)
                    {
                        int twoSquareY = ParentCell.Y + (2 * direction);
                        if (!board.IsOffBoard(ParentCell.X, twoSquareY))
                        {
                            Cell twoSquareCell = board.GetCell(ParentCell.X, twoSquareY);
                            if (twoSquareCell.IsEmpty)
                            {
                                moves.Add(new Move(ParentCell, twoSquareCell));
                            }
                        }
                    }
                }
            }

            // Diagonal captures
            int[] captureX = { ParentCell.X - 1, ParentCell.X + 1 };
            foreach (int x in captureX)
            {
                int captureY = ParentCell.Y + direction;
                if (!board.IsOffBoard(x, captureY))
                {
                    Cell captureCell = board.GetCell(x, captureY);
                    if (IsOpponent(captureCell))
                    {
                        moves.Add(new Move(ParentCell, captureCell));
                    }
                }
            }

            // En passant
            if (board.LastMove != null && board.LastMove.PieceMoved is Pawn)
            {
                // Check if last move was opponent pawn double move
                Pawn? lastMovedPawn = board.LastMove.PieceMoved as Pawn;
                if (lastMovedPawn != null && lastMovedPawn.Color != Color)
                {
                    // Check if opponent pawn moved two squares
                    int twoSquareDiff = Math.Abs(board.LastMove.From.Y - board.LastMove.To.Y);
                    if (twoSquareDiff == 2 && board.LastMove.To.Y == ParentCell.Y)
                    {
                        // Opponent pawn is on same rank, check if adjacent file
                        if (Math.Abs(board.LastMove.To.X - ParentCell.X) == 1)
                        {
                            int enPassantY = ParentCell.Y + direction;
                            Cell enPassantCell = board.GetCell(board.LastMove.To.X, enPassantY);
                            if (!board.IsOffBoard(board.LastMove.To.X, enPassantY))
                            {
                                Move enPassantMove = new Move(ParentCell, enPassantCell);
                                enPassantMove.IsEnPassantMove = true;
                                moves.Add(enPassantMove);
                            }
                        }
                    }
                }
            }

            return moves;
        }
    }
}
