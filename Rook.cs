using System;
using System.Collections.Generic;
namespace ConsoleChess
{
    public class Rook : SlidingPiece
    {
        public override char Symbol => Color == PlayerColor.White ? '♖' : '♜';

        public Rook(PlayerColor color) : base(color) { }

        internal override List<Move> GetPseudoLegalMoves(ChessBoard board)
        {
            return CalculateSliding(board, Direction.Orthogonal);
        }
    }
}
