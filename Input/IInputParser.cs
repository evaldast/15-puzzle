using Puzzle.Enums;

namespace Puzzle.Input
{
    public interface IInputParser
    {
        Direction RequestInput();
    }
}