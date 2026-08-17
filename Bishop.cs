using System;
using System.Collections.Generic;
namespace ConsoleChess
{
    public class Bishop : SlidingPiece
    {
        public override char Symbol => Color == PlayerColor.White ? '♗' : '♝';

        public Bishop(PlayerColor color) : base(color) { }

        internal override List<Move> GetPseudoLegalMoves(ChessBoard board)
        {
            return CalculateSliding(board, Direction.Diagonal);
        }
    }
}
