using System;
namespace ConsoleChess
{
    public class Cell
    {   //coordinates
        public int X { get; }
        public int Y { get; }
        //occupancy
        public Piece? Piece { get; set; }
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            Piece = null;
        }
        //place a piece and update its knowledge of location
        public void Occupy(Piece piece)
        {
            Piece = piece;
            piece.ParentCell = this;
        }
        public void Clear()
        {
            if (Piece != null)
            {
                Piece.ParentCell = null;
                Piece = null;
            }
        }
        //helper, make logic elsewhere
        public bool IsEmpty => Piece == null;//instant state check,read naturally
    }
}
