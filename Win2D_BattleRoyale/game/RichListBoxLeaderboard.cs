using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas;

namespace Win2D_BattleRoyale
{
    public class RichListBoxLeaderboard : RichListBox
    {
        private Vector2 LeadersPosition { get; set; }
        private CanvasTextFormat LeadersFont { get; set; }
        private CanvasTextLayout LeadersTextLayout { get; set; }

        private List<IRichString> Leaders = new List<IRichString>();

        private int MaxY { get; set; }

        public RichListBoxLeaderboard(CanvasDevice device, Vector2 position, int width, int height, string title, CanvasTextFormat titleFont, CanvasTextFormat stringsFont, CanvasTextFormat leadersFont)
            : base(device, position, width, height, title, titleFont, stringsFont, 0)
        {
            LeadersFont = leadersFont;

            // calculate leaders text layout (for text height)
            LeadersTextLayout = new CanvasTextLayout(device, "ABCDEFG", LeadersFont, 0, 0);

            // calculate leaders position Y
            LeadersPosition = new Vector2(0, StringsPosition.Y);

            // leaders position is dynamic
            // strings position is dynamic
        }

        public override void Draw(CanvasAnimatedDrawEventArgs args)
        {
            base.Draw(args);

            // draw leaders

            // draw first column of strings

            // draw second column of strings
        }
    }
}
