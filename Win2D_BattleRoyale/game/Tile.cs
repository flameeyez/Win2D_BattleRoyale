using Windows.Foundation;

namespace Win2D_BattleRoyale
{
    public class Tile
    {
        public Point Coordinates { get; set; }
        public Region Region { get; set; }
        public bool Available { get; set; }

        public Tile(Point coordinates)
        {
            Region = null;
            Coordinates = coordinates;
            Available = true;
        }
    }
}
