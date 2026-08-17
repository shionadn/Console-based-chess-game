using System.Collections.Generic;
using System;

namespace ConsoleChess
{
    public class ChessBoard
    {   //1. properties
        public Cell[,] Grid { get; } = new Cell[8, 8];
        public PlayerColor CurrentTurn { get; private set; }
        //locates pieces without scanning the 8x8 grid
        public List<Piece> ActivePieces { get; set; }
        public Move? LastMove { get; set; }
        public PlayerColor CurrentPlayer => CurrentTurn;

        private readonly MoveValidator moveValidator = new MoveValidator();
        private readonly MoveParser inputParser = new MoveParser();

        public ChessBoard()
            : this(false)
        {
        }

        private ChessBoard(bool skipSetup)
        {
            ActivePieces = new List<Piece>();

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Grid[x, y] = new Cell(x, y);
                }
            }
            //adding pieces to the board
            if (!skipSetup)
            {
                SetupStandardBoard();
            }
            //white starts first - removed parentheses as PlayerColor.White is an enum value
            CurrentTurn = PlayerColor.White;
        }

        public void SetupStandardBoard()
        {
            foreach (Cell cell in Grid)
            {
                cell.Clear();
            }

            ActivePieces.Clear();
            LastMove = null;
            SetUpNewGames();
            CurrentTurn = PlayerColor.White;
        }

        private void SetUpNewGames()
        {

            //White
            InitializeRank(0, PlayerColor.White, true);
            InitializeRank(1, PlayerColor.White, false);
            //Black
            InitializeRank(7, PlayerColor.Black, true);
            InitializeRank(6, PlayerColor.Black, false);
        }

        private void InitializeRank(int rank, PlayerColor color, bool IsBackRanked)
        {
            if (IsBackRanked)
            {
                PlacePiece(new Rook(color), 0, rank);
                PlacePiece(new Knight(color), 1, rank);
                PlacePiece(new Bishop(color), 2, rank);
                PlacePiece(new Queen(color), 3, rank);
                PlacePiece(new King(color), 4, rank);
                PlacePiece(new Bishop(color), 5, rank);
                PlacePiece(new Knight(color), 6, rank);
                PlacePiece(new Rook(color), 7, rank);
            }
            else
            {
                for (int x = 0; x < 8; x++)
                {
                    PlacePiece(new Pawn(color), x, rank);
                }
            }
        }

        //connect Piece, Cell, ActivePieces - renamed to PlacePiece to match your calls
        private void PlacePiece(Piece piece, int x, int y)
        {
            Cell target = Grid[x, y];
            target.Occupy(piece);//place piece into target cell, update ParentCell
            ActivePieces.Add(piece);
        }

        //2. methods for managing the board
        public Cell? GetCell(int x, int y)
        {
            if (IsOffBoard(x, y))
            { return null; }
            return Grid[x, y];
        }

        public bool IsOffBoard(int x, int y)
        {
            return x < 0 || x > 7 || y < 0 || y > 7;
        }

        public void SwitchTurn()
        {
            CurrentTurn = (CurrentTurn == PlayerColor.White) ? PlayerColor.Black : PlayerColor.White;
        }

        public Move? ParseInputToMove(string fromStr, string toStr)
        {
            return inputParser.Parse(fromStr, toStr, this);
        }

        public Move? ParseInputToMove(string input)
        {
            return inputParser.Parse(input, this);
        }

        public Move? GetMove(int fromX, int fromY, int toX, int toY)
        {
            if (IsOffBoard(fromX, fromY) || IsOffBoard(toX, toY))
            {
                return null;
            }

            return new Move(GetCell(fromX, fromY), GetCell(toX, toY));
        }

        public bool IsLegalMove(Move move)
        {
            if (move == null || move.From == null || move.To == null || move.From.IsEmpty)
            {
                return false;
            }

            if (move.From.Piece.Color != CurrentTurn)
            {
                return false;
            }

            if (!move.To.IsEmpty && move.To.Piece.Color == move.From.Piece.Color)
            {
                return false;
            }

            return moveValidator.IsLegalMove(this, move);
        }

        public bool HasAnyLegalMoves(PlayerColor playerColor)
        {
            foreach (Piece piece in ActivePieces)
            {
                if (piece.Color != playerColor)
                {
                    continue;
                }

                if (moveValidator.GetLegalMoves(this, piece).Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsCheckmate(PlayerColor playerColor)
        {
            return IsKingInCheck(playerColor) && !HasAnyLegalMoves(playerColor);
        }

        public bool IsStalemate(PlayerColor playerColor)
        {
            return !IsKingInCheck(playerColor) && !HasAnyLegalMoves(playerColor);
        }

        public void ExecuteMove(Move move)
        {
            ApplyMove(move, true);
        }

        public void SimulateMoves(Move move)
        {
            ApplyMove(move, false);
        }

        private void ApplyMove(Move move, bool promptPromotion)
        {
            //get in4 for moving and capture piece
            Piece movingPiece = move.From.Piece;
            Piece capturedPiece = move.To.Piece;

            if (movingPiece == null)
            {
                return;
            }

            //1. En Passant check
            if (move.IsEnPassantMove)
            {
                //coordination of opponent's pawn
                //From=oldX, To=newY
                int victimX = move.To.X;
                int victimY = move.From.Y;
                Cell? victimCell = GetCell(victimX, victimY);

                //eliminate opponent's pawn from ActivePiece List
                if (victimCell != null && victimCell.Piece != null)
                {
                    ActivePieces.Remove(victimCell.Piece);
                    victimCell.Clear();
                }
            }

            //move pieces on grid

            move.From.Clear();
            move.To.Occupy(movingPiece);
            movingPiece.HasMoved = true;
            LastMove = move;

            //if captured, eliminate from ActivePiece List and KingInCheck
            if (capturedPiece != null && !move.IsEnPassantMove)
            {
                ActivePieces.Remove(capturedPiece);
            }

            //2. Promotion check
            if (movingPiece is Pawn)
            {
                if (move.To.Y == 0 || move.To.Y == 7)
                {
                    HandlePromotion(move.To, promptPromotion);
                }
            }
            SwitchTurn();
        }

        private void HandlePromotion(Cell cell, bool promptPromotion)
        {
            //gets previous pawn's color
            PlayerColor color = cell.Piece.Color;
            //eliminate previous pawn from ActivePiece List
            ActivePieces.Remove(cell.Piece);

            Piece? newPiece = null;

            if (promptPromotion)
            {
                Console.WriteLine("\n    PAWN PROMOTION!    ");
                Console.WriteLine("Choose a piece: (Q)ueen, (R)ook, (B)ishop, (K)night");

                bool validChoice = false;
                while (!validChoice)
                {
                    Console.Write("Enter your choice (Q/R/B/K): ");
                    string choice = Console.ReadLine()?.ToUpperInvariant();
                    switch (choice)
                    {
                        case "Q":
                            newPiece = new Queen(color);
                            validChoice = true;
                            break;
                        case "R":
                            newPiece = new Rook(color);
                            validChoice = true;
                            break;
                        case "B":
                            newPiece = new Bishop(color);
                            validChoice = true;
                            break;
                        case "K":
                            newPiece = new Knight(color);
                            validChoice = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please press Q, R, B, or K.");
                            break;
                    }
                }
            }
            else
            {
                newPiece = new Queen(color);
            }

            //put the promoted piece onto the square and update ActivePiece List
            if (newPiece == null)
            {
                return;
            }

            cell.Occupy(newPiece);
            ActivePieces.Add(newPiece);
            if (promptPromotion)
            {
                Console.WriteLine($"Pawn has promoted to {newPiece.GetType().Name}");
            }
        }

        public bool IsKingInCheck(PlayerColor kingColor)
        {
            //1. locate: find cell contains your side king's color
            Cell? kingCell = null;
            foreach (var piece in ActivePieces)
            {
                if (piece is King && piece.Color == kingColor)
                {
                    kingCell = piece.ParentCell;
                    break;
                }
            }
            //null case
            if (kingCell == null) return false;

            //2. define the opponent's color
            PlayerColor opponentColor = (kingColor == PlayerColor.White) ? PlayerColor.Black : PlayerColor.White;

            //3. scan whether any of enemy's pieces can capture your king
            foreach (var piece in ActivePieces)
            {
                if (piece.Color == opponentColor)
                {
                    //get all pseudomove of the enemy
                    List<Move> enemyMoves = piece.GetPseudoLegalMoves(this);

                    foreach (var move in enemyMoves)
                    {   //if destination==kingCell's coordinates
                        if (move.To.X == kingCell.X && move.To.Y == kingCell.Y)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public ChessBoard Clone()
        {
            ChessBoard clone = new ChessBoard(true);
            clone.CurrentTurn = CurrentTurn;

            foreach (Piece piece in ActivePieces)
            {
                Piece clonedPiece = ClonePiece(piece);
                clone.PlacePiece(clonedPiece, piece.ParentCell.X, piece.ParentCell.Y);
            }

            if (LastMove != null)
            {
                clone.LastMove = clone.GetMove(LastMove.From.X, LastMove.From.Y, LastMove.To.X, LastMove.To.Y);
                clone.LastMove.IsCastling = LastMove.IsCastling;
                clone.LastMove.IsEnPassantMove = LastMove.IsEnPassantMove;
                clone.LastMove.IsPromotionMove = LastMove.IsPromotionMove;
            }

            return clone;
        }

        private static Piece ClonePiece(Piece piece)
        {
            Piece clonedPiece = piece switch
            {
                Pawn => new Pawn(piece.Color),
                Rook => new Rook(piece.Color),
                Knight => new Knight(piece.Color),
                Bishop => new Bishop(piece.Color),
                Queen => new Queen(piece.Color),
                King => new King(piece.Color),
                _ => throw new InvalidOperationException("Unsupported piece type.")
            };

            clonedPiece.HasMoved = piece.HasMoved;
            return clonedPiece;
        }
    }
}
