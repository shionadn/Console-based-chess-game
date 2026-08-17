using System;
using System.Collections.Generic;
namespace ConsoleChess
{
    public class Knight : SteppingPiece
    {
        public override char Symbol => Color == PlayerColor.White ? '♘' : '♞';

        public Knight(PlayerColor color) : base(color) { }

        internal override List<Move> GetPseudoLegalMoves(ChessBoard board)
        {
            (int dx, int dy)[] knightMoves =
            {
                (2, 1), (2, -1), (-2, 1), (-2, -1),
                (1, 2), (1, -2), (-1, 2), (-1, -2)
            };
            return CalculateStepping(board, knightMoves);
        }
    }
}
