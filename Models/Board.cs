using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Puzzle.Enums;

namespace Puzzle.Models
{
    public class Board
    {
        private const int BoardWidth = 4;
        private const int ShuffleThoroughness = 500;
        private readonly BoardPiece[][] _boardState = InitializeBoardPieces();
        private static (int x, int y) _emptyBoardPieceLocation = (x: BoardWidth - 1, y: BoardWidth - 1);

        public ReadOnlyCollection<BoardPiece[]> CurrentBoardState => new ReadOnlyCollection<BoardPiece[]>(_boardState);
        public bool Solved => _boardState.All(row => row.SequenceEqual(row.OrderBy(x => x.Value)));

        public IEnumerable<Direction> GetAvailableMoves()
        {
            return new List<Direction>((Direction[]) Enum.GetValues(typeof(Direction)));
        }

        public void MovePiece(Direction direction)
        {
            MovePiece(direction, CurrentBoardState[_emptyBoardPieceLocation.x][_emptyBoardPieceLocation.y]);
        }

        private void MovePiece(Direction direction, BoardPiece pieceToMove)
        {
            switch (direction)
            {
                case Direction.Up:
                {
                    SwapPieces(pieceToMove, -1, 0);

                    break;
                }
                case Direction.Down:
                {
                    SwapPieces(pieceToMove, 1, 0);

                    break;
                }
                case Direction.Left:
                {
                    SwapPieces(pieceToMove, 0, -1);

                    break;
                }
                case Direction.Right:
                {
                    SwapPieces(pieceToMove, 0, 1);

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction,
                        Error.InvalidDirection.ToString());
            }
        }

        private void SwapPieces(BoardPiece pieceToMove, int moveByX, int moveByY)
        {
            if ((pieceToMove.Location.x + moveByX < 0 || pieceToMove.Location.x + moveByX >= BoardWidth) ||
                (pieceToMove.Location.y + moveByY < 0 || pieceToMove.Location.y + moveByY >= BoardWidth))
            {
                throw new ArgumentException(Error.InvalidDirection.ToString());
            }

            BoardPiece initialBoardPiece = _boardState[pieceToMove.Location.x][pieceToMove.Location.y];
            BoardPiece pieceToSwap = _boardState[pieceToMove.Location.x + moveByX][pieceToMove.Location.y + moveByY];

            _boardState[pieceToMove.Location.x][pieceToMove.Location.y] = pieceToSwap;
            _boardState[pieceToSwap.Location.x][pieceToSwap.Location.y] = initialBoardPiece;

            (int x, int y) pieceToMoveLocation = initialBoardPiece.Location;
            (int x, int y) pieceToSwapLocation = pieceToSwap.Location;

            initialBoardPiece.Location = pieceToSwapLocation;
            pieceToSwap.Location = pieceToMoveLocation;

            if (pieceToMove is EmptyBoardPiece)
            {
                _emptyBoardPieceLocation = initialBoardPiece.Location;
            }
        }

        public void Shuffle()
        {
            var rnd = new Random();

            for (var i = 1; i <= ShuffleThoroughness; i++)
            {
                var pieceToMove = _boardState[rnd.Next(0, BoardWidth)][rnd.Next(0, BoardWidth)];

                try
                {
                    MovePiece((Direction) rnd.Next(0, Enum.GetNames(typeof(Direction)).Length), pieceToMove);
                }
                catch
                {
                    continue;
                }

                if (pieceToMove is EmptyBoardPiece)
                {
                    _emptyBoardPieceLocation = pieceToMove.Location;
                }
            }

            SwapEmptyPieceToBottomRightCorner();

            if (!Solvable())
            {
                MovePiece(Direction.Right, _boardState[0][0]);
            }
        }

        private void SwapEmptyPieceToBottomRightCorner()
        {
            foreach (BoardPiece[] row in _boardState)
            {
                foreach (BoardPiece piece in row)
                {
                    if (!(piece is EmptyBoardPiece))
                    {
                        continue;
                    }

                    if (piece.Location.x != 3 || piece.Location.y != 3)
                    {
                        SwapPieces(piece, 3 - piece.Location.x, 3 - piece.Location.y);
                    }

                    break;
                }
            }
        }

        private bool Solvable()
        {
            return CalculateInversions() % 2 == 0;
        }

        private int CalculateInversions()
        {
            var flattenedBoardValues = _boardState.SelectMany(x => x).Select(x => x.Value).ToArray();
            var sumOfInversions = 0;

            for (var i = 0; i < flattenedBoardValues.Length - 1; i++)
            {
                for (var j = i + 1; j < flattenedBoardValues.Length; j++)
                {
                    if (flattenedBoardValues[i] > flattenedBoardValues[j])
                    {
                        sumOfInversions++;
                    }
                }
            }

            return sumOfInversions;
        }

        private static BoardPiece[][] InitializeBoardPieces()
        {
            var board = new BoardPiece[BoardWidth][];
            var currentValue = 1;

            for (var i = 0; i < BoardWidth; i++)
            {
                board[i] = new BoardPiece[BoardWidth];

                for (var j = 0; j < BoardWidth; j++)
                {
                    board[i][j] = new BoardPiece(i, j, currentValue++);
                }
            }

            board[_emptyBoardPieceLocation.x][_emptyBoardPieceLocation.y] =
                new EmptyBoardPiece(_emptyBoardPieceLocation.x, _emptyBoardPieceLocation.y);

            return board;
        }
    }
}