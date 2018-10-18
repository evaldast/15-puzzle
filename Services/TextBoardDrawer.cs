using System;
using System.Collections.Generic;
using Puzzle.Enums;
using Puzzle.Models;

namespace Puzzle.Services
{
    public class TextBoardDrawer : IBoardDrawer
    {
        public void DrawBoard(IEnumerable<BoardPiece[]> board)
        {
            foreach (BoardPiece[] row in board)
            {
                var stringifiedBoardRepresentation = string.Empty;

                foreach (BoardPiece piece in row)
                {
                    if (piece is EmptyBoardPiece)
                    {
                        stringifiedBoardRepresentation += $"|  {piece}   ";

                        continue;
                    }
                    
                    stringifiedBoardRepresentation += piece.Value < 10
                        ? $"|  {piece}  "
                        : $"| {piece}  ";
                }

                stringifiedBoardRepresentation += "|";

                Console.WriteLine(stringifiedBoardRepresentation);
            }
        }

        public void DrawAvailableMoves(IEnumerable<Direction> availableMoves)
        {
            Console.WriteLine("Please type in one of the directions:");
            
            foreach (var move in availableMoves)
            {
                Console.WriteLine(move.ToString());                
            }
            
            Console.WriteLine();
        }
    }
}