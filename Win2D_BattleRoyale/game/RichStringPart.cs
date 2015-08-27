using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Numerics;
using Windows.UI;

namespace Win2D_BattleRoyale
{
    public class RichStringPart
    {
        public string String { get; set; }
        public Color Color { get; set; }

        public RichStringPart(string str, Color color)
        {
            String = str;
            Color = color;
        }

        public RichStringPart(string str)
        {
            String = str;
            Color = Colors.White;
        }

        public void Draw(CanvasAnimatedDrawEventArgs args, Vector2 position, CanvasTextFormat font)
        {
            args.DrawingSession.DrawText(String, position, Color, font);
        }
    }
}
