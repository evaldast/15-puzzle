using System.Collections.Generic;
using Puzzle.Enums;
using Puzzle.Models;

namespace Puzzle.Services
{
    public interface IBoardDrawer
    {
        void DrawBoard(IEnumerable<BoardPiece[]> board);
        void DrawAvailableMoves(IEnumerable<Direction> getAvailableMoves);
    }
}