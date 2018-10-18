using Puzzle.Enums;

namespace Puzzle.Services
{
    public interface IInputParser
    {
        Direction RequestInput();
    }
}