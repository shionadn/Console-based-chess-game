using System;
using System.Collections.Generic;
namespace ConsoleChess
{
    public class King : SteppingPiece
    {
        public override char Symbol => Color == PlayerColor.White ? '♔' : '♚';

        public King(PlayerColor color) : base(color) { }

        internal override List<Move> GetPseudoLegalMoves(ChessBoard board)
        {
            (int dx, int dy)[] kingMoves =
            {
                (1, 0), (-1, 0), (0, 1), (0, -1),
                (1, 1), (1, -1), (-1, 1), (-1, -1)
            };
            return CalculateStepping(board, kingMoves);
        }
    }
}
