namespace MarsRovers
{
    public class RoverPosition
    {
        public enum CardinalPoint { N, E, S, W }

        public int X { get; set; }

        public int Y { get; set; }

        public CardinalPoint Direction { get; set; }

        public RoverPosition(int x, int y, CardinalPoint direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }
    }
}
