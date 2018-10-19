using Puzzle.Input;
using Puzzle.Render;

namespace Puzzle
{
    class Program
    {
        static void Main(string[] args) => new PuzzleGame(new TerminalInputParser(), new TextBoardDrawer()).StartGame();
    }
}