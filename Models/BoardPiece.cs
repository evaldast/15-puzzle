namespace Puzzle.Models
{
    public class BoardPiece
    {
        public int Value { get; }
        //public (int x, int y) Location { get; set; }

        public BoardPiece(int xCoord, int yCoord, int value)
        {
            //Location = (x: xCoord, y: yCoord);
            Value = value;
        }

        public override string ToString()
        {
            return this is EmptyBoardPiece ? string.Empty : Value.ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BoardPiece pieceToCompare))
            {
                return false;
            }

            return Value.Equals(pieceToCompare.Value);
        }
        
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }                
    }
}