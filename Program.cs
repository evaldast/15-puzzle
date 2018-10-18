using Puzzle.Services;

namespace Puzzle
{
    class Program
    {
        static void Main(string[] args) => new PuzzleGame(new TerminalInputParser(), new TextBoardDrawer()).StartGame();
    }
}