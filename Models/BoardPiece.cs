namespace Puzzle.Models
{
    public class BoardPiece
    {
        public bool Visible { get; }
        public int Value { get; }
        public (int x, int y) Location { get; set; }

        public BoardPiece(int xCoord, int yCoord, int value, bool visible = true)
        {
            Location = (x: xCoord, y: yCoord);
            Value = value;
            Visible = visible;
        }

        public override string ToString()
        {
            return Visible ? Value.ToString() : string.Empty;
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