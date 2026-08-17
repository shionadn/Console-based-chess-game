using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleChess
{
    public class MoveValidator
    {
        //pinned|incheck|sabotage
        //filtering:PseudoLegalMoves,SimulateMoves,IsKingInCheck
        internal List<Move> GetLegalMoves(ChessBoard board, Piece piece)
        {   //1. gets all seemingly right moves according to the moves of pieces
            List<Move> pseudoMoves = piece.GetPseudoLegalMoves(board);//not considering IsKingInCheck
            List<Move> legalMoves = new List<Move>();//considering IsKingInCheck
            foreach (var move in pseudoMoves)
            {
                //2. simulate: try this move on board
                ChessBoard simulationBoard = board.Clone();
                Move simulatedMove = simulationBoard.GetMove(move.From.X, move.From.Y, move.To.X, move.To.Y);
                simulationBoard.SimulateMoves(simulatedMove);
                //3. Check if your color's IsKingInCheck after move
                if (!simulationBoard.IsKingInCheck(piece.Color))
                {
                    legalMoves.Add(move);
                }
            }
            return legalMoves;
        }

        public bool IsLegalMove(ChessBoard board, Move move)
        {
            if (move == null || move.From == null || move.To == null || move.From.IsEmpty)
            {
                return false;
            }

            return GetLegalMoves(board, move.From.Piece).Any(candidate =>
                candidate.From.X == move.From.X &&
                candidate.From.Y == move.From.Y &&
                candidate.To.X == move.To.X &&
                candidate.To.Y == move.To.Y);
        }
    }
}
