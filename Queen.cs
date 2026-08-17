using System;
using System.Collections.Generic;
namespace ConsoleChess
{
    public class Queen : SlidingPiece
    {
        public override char Symbol => Color == PlayerColor.White ? '♕' : '♛';

        public Queen(PlayerColor color) : base(color) { }

        internal override List<Move> GetPseudoLegalMoves(ChessBoard board)
        {
            return CalculateSliding(board, Direction.AllEight);
        }
    }
}
