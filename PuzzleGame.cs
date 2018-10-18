using System;
using Puzzle.Models;
using Puzzle.Services;
using static System.Console;

namespace Puzzle
{
    public class PuzzleGame
    {
        private readonly IInputParser _inputParser;
        private readonly IBoardDrawer _boardDrawer;
        private readonly Board _board;

        public PuzzleGame(IInputParser inputParser, IBoardDrawer boardDrawer)
        {
            _inputParser = inputParser;
            _boardDrawer = boardDrawer;
            _board = new Board();
        }

        public void StartGame()
        {
            _board.Shuffle();

            do
            {
                try
                {
                    _boardDrawer.DrawBoard(_board.CurrentBoardState);
                    _boardDrawer.DrawAvailableMoves(_board.GetAvailableMoves());

                    _board.Move(_inputParser.RequestInput());
                }
                catch (Exception e)
                {
                    WriteLine();
                    WriteLine(e.Message);
                    WriteLine();
                }
            } while (!_board.Solved);
        }
    }
}