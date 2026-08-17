using System;
namespace ConsoleChess
{
    public class Move
    {
        public Cell From { get; }
        public Cell To { get; }
        public Piece PieceMoved { get; }
        public Piece? PieceCaptured { get; }

        public bool IsPromotionMove { get; set; }
        public bool IsEnPassantMove { get; set; }
        public bool IsCastling { get; set; }

        public Move(Cell from, Cell to)
        {
            From = from;
            To = to;
            PieceMoved = from.Piece;
            PieceCaptured = to.Piece;
        }
    }
}
