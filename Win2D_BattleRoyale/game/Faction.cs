using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Win2D_BattleRoyale
{
    public class Faction
    {
        public string Name { get; set; }
        public Leader Leader { get; set; }
        public Color Color { get; set; }
        public List<Region> Regions = new List<Region>();

        public void Draw(Vector2 MapPosition, CanvasAnimatedDrawEventArgs args)
        {
            foreach(Region region in Regions)
            {
                region.Draw(MapPosition, args);
            }
        }
    }
}