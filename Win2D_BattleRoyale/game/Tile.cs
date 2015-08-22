using Windows.Foundation;

namespace Win2D_BattleRoyale
{
    public class Tile
    {
        public Point Coordinates { get; set; }
        public int Region { get; set; }
        public bool Available { get; set; }

        public Tile(Point coordinates)
        {
            Coordinates = coordinates;
            Available = true;
        }
    }
}
