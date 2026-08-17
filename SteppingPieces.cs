using System;
using System.Collections.Generic;
namespace ConsoleChess
{
    public abstract class SteppingPiece : Piece
    {
        public SteppingPiece(PlayerColor color) : base(color)
        {
        }
        protected List<Move> CalculateStepping(ChessBoard board, (int dx, int dy)[] displacementVector)
        {
            var legalMoves = new List<Move>();
            foreach (var v in displacementVector)
            {
                //translated coordinates: P' = P + v
                int targetX = ParentCell.X + v.dx;
                int targetY = ParentCell.Y + v.dy;

                if (board.IsOffBoard(targetX, targetY))
                {
                    continue;
                }
                Cell targetCell = board.GetCell(targetX, targetY);
                if (targetCell.IsEmpty || IsOpponent(targetCell))
                {
                    legalMoves.Add(new Move(ParentCell, targetCell));
                }
            }
            return legalMoves;
        }
    }
}