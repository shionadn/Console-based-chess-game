using System;
using System.Collections.Generic;
namespace ConsoleChess
{
    public abstract class SlidingPiece : Piece
    {
        public SlidingPiece(PlayerColor color) : base(color)
        {
        }
        protected List<Move> CalculateSliding(ChessBoard board, (int x, int y)[] directions)
        {
            var moves = new List<Move>();
            //outerloop:direction
            foreach (var direction in directions)
            {
                //innerloop:scan up to 7 squares away
                for (int i = 1; i < 8; i++)
                {
                    int targetX = ParentCell.X + direction.x * i;
                    int targetY = ParentCell.Y + direction.y * i;
                    //condition 1: within grid's range
                    if (board.IsOffBoard(targetX, targetY))
                        break;
                    Cell targetCell = board.GetCell(targetX, targetY);
                    //condition 2: empty/without obstacle
                    if (targetCell.IsEmpty)
                    {
                        moves.Add(new Move(ParentCell, targetCell));
                    }
                    //condition 3: with obstacle
                    else
                    {
                        if (IsOpponent(targetCell))
                        {
                            moves.Add(new Move(ParentCell, targetCell));
                        }
                        break;
                    }
                }
            }
            return moves;
        }
    }
}