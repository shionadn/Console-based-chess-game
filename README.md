
# ConsoleChess

A lightweight terminal-based chess game implemented in C#.

ConsoleChess lets you play a full game of chess directly in the command line, with standard movement rules, turn handling, legal-move validation, check detection, checkmate, stalemate, and pawn promotion.

## Features

- 8x8 chess board rendered in the console
- Unicode chess piece symbols
- Legal move validation using the board state
- Check, checkmate, and stalemate detection
- Pawn promotion to Queen, Rook, Bishop, or Knight
- Move parsing for common input styles such as:
  - `e2 e4`
  - `pawn to d4`
  - `d4`
  - `a2 a3`
- Simple command-line gameplay loop

## Requirements

- .NET 10 SDK

## Running the game

From the repository root:

```bash
dotnet restore
dotnet run
```

## How to play

When the program starts, it displays the board and prompts you for a move.

Example inputs:

```text
e2 e4
pawn to d4
c2 c3
```

You can also quit the game anytime by entering:

```text
exit
```

## Project structure

- `Program.cs` — main game loop and user interaction
- `ChessBoard.cs` — board state, turn logic, and move execution
- `BoardRenderer.cs` — console rendering of the board
- `MoveValidator.cs` — legal-move filtering
- `ParseInputToMove.cs` — parsing of human-friendly move commands
- `Move.cs`, `Cell.cs`, `Piece.cs` — core chess mechanics and data models
- `SlidingPieces.cs`, `SteppingPieces.cs` — movement logic for different piece types

## Notes

This project is a small, educational chess implementation focused on console interaction and chess rules logic rather than advanced engine features or GUI interfaces.

