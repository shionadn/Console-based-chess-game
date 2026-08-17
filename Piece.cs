using System;
using System.Collections.Generic;
namespace ConsoleChess
{
    public abstract class Piece
    {
        public PlayerColor Color { get; }
        public abstract char Symbol { get; }
        public Cell? ParentCell { get; set; }
        public bool HasMoved { get; set; } = false;
        protected Piece(PlayerColor color)
        {
            Color = color;
        }
        public PlayerColor PieceColor => Color;
        internal abstract List<Move> GetPseudoLegalMoves(ChessBoard board);
        protected bool IsOnBoard(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }
        protected bool IsOpponent(Cell cell)
        {
            return !cell.IsEmpty && cell.Piece.Color != this.Color;
        }
    }
}
