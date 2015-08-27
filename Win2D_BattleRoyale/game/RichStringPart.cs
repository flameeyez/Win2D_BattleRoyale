using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI;

namespace Win2D_BattleRoyale
{
    public class RichStringPart
    {
        public string String { get; set; }
        public Color Color { get; set; }
        public CanvasTextFormat Font { get; set; }
        private CanvasTextLayout Layout { get; set; }

        public RichStringPart(string str, Color color, CanvasTextFormat font, CanvasAnimatedDrawEventArgs args)
        {
            String = str;
            Color = color;
            Font = font;

            Layout = new CanvasTextLayout(args.DrawingSession, str, font, 0, 0);
        }

        public RichStringPart(string str)
        {
            String = str;
            Color = Colors.White;
        }

        public void Draw()
        {

        }
    }
}
