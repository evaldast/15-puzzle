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
            MovePiece(direction, _emptyBoardPieceLocation);
        }

        private void MovePiece(Direction direction, (int x, int y) pieceToMoveLocation)
        {
            switch (direction)
            {
                case Direction.Up:
                {
                    SwapPieces(pieceToMoveLocation, -1, 0);

                    break;
                }
                case Direction.Down:
                {
                    SwapPieces(pieceToMoveLocation, 1, 0);

                    break;
                }
                case Direction.Left:
                {
                    SwapPieces(pieceToMoveLocation, 0, -1);

                    break;
                }
                case Direction.Right:
                {
                    SwapPieces(pieceToMoveLocation, 0, 1);

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction,
                        Error.InvalidDirection.ToString());
            }
        }

        private void SwapPieces((int x, int y) pieceToMoveLocation, int moveByX, int moveByY)
        {
            if (!SwapIsValid(pieceToMoveLocation, moveByX, moveByY))
            {
                throw new ArgumentException(Error.InvalidDirection.ToString());
            }

            BoardPiece pieceToMove = _boardState[pieceToMoveLocation.x][pieceToMoveLocation.y];

            _boardState[pieceToMoveLocation.x][pieceToMoveLocation.y] = _boardState[pieceToMoveLocation.x + moveByX][pieceToMoveLocation.y + moveByY];
            _boardState[pieceToMoveLocation.x + moveByX][pieceToMoveLocation.y + moveByY] = pieceToMove;

            if (pieceToMove is EmptyBoardPiece)
            {
                _emptyBoardPieceLocation = (x: pieceToMoveLocation.x + moveByX, y: pieceToMoveLocation.y + moveByY);
            }
        }

        private bool SwapIsValid((int x, int y) pieceToMoveLocation, int moveByX, int moveByY)
        {
            return ((pieceToMoveLocation.x + moveByX < 0 || pieceToMoveLocation.x + moveByX >= BoardWidth) ||
                    (pieceToMoveLocation.y + moveByY < 0 || pieceToMoveLocation.y + moveByY >= BoardWidth));
        }

        public void Shuffle()
        {
            var rnd = new Random();

            for (var i = 1; i <= ShuffleThoroughness; i++)
            {
                (int x, int y) pieceToMoveLocation = (x: rnd.Next(0, BoardWidth), y: rnd.Next(0, BoardWidth));

                try
                {
                    MovePiece((Direction) rnd.Next(0, Enum.GetNames(typeof(Direction)).Length), pieceToMoveLocation);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            SwapEmptyPieceToBottomRightCorner();

            if (!Solvable())
            {
                MovePiece(Direction.Right, (x: 0, y: 0));
            }
        }

        private void SwapEmptyPieceToBottomRightCorner()
        {
            for (var rowIndex = 0; rowIndex < BoardWidth; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < BoardWidth; columnIndex++)
                {
                    if (!(_boardState[rowIndex][columnIndex] is EmptyBoardPiece))
                    {
                        continue;
                    }

                    if (rowIndex == BoardWidth - 1 && columnIndex == BoardWidth - 1)
                    {
                        return;
                    }

                    SwapPieces((x: rowIndex, y: columnIndex), (BoardWidth - 1) - rowIndex,
                        (BoardWidth - 1) - columnIndex);

                    return;
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

            for (var rowIndex = 0; rowIndex < BoardWidth; rowIndex++)
            {
                board[rowIndex] = new BoardPiece[BoardWidth];

                for (var columnIndex = 0; columnIndex < BoardWidth; columnIndex++)
                {
                    board[rowIndex][columnIndex] = new BoardPiece(rowIndex, columnIndex, currentValue++);
                }
            }

            board[_emptyBoardPieceLocation.x][_emptyBoardPieceLocation.y] = new EmptyBoardPiece(BoardWidth - 1, BoardWidth - 1);

            return board;
        }
    }
}