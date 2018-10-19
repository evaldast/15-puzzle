using System;
using Puzzle.Enums;

namespace Puzzle.Input
{
    public class TerminalInputParser : IInputParser
    {
        public Direction RequestInput() => ParseTextInput(Console.ReadLine());

        private static Direction ParseTextInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || !Enum.TryParse(input, true, out Direction result))
            {
                throw new ArgumentException(Error.InvalidInput.ToString());
            }
                
            return result;
        }
    }
}