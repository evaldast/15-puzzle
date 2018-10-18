namespace Puzzle.Models
{
    public class EmptyBoardPiece : BoardPiece
    {
        public EmptyBoardPiece(int xCoord, int yCoord) : base(xCoord, yCoord, int.MaxValue)
        {
        }
    }
}