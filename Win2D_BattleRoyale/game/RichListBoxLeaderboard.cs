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

        private static CanvasTextLayout NoLeadersTextLayout { get; set; }

        // subset of Strings (cast as Leaders, most wins)
        public List<Leader> Leaders = new List<Leader>();

        private int MaxY { get; set; }

        public RichListBoxLeaderboard(CanvasDevice device, Vector2 position, int width, int height, string title, CanvasTextFormat titleFont, CanvasTextFormat stringsFont, CanvasTextFormat leadersFont)
            : base(device, position, width, height, title, titleFont, stringsFont, 0)
        {
            LeadersFont = leadersFont;

            // calculate leaders text layout (for text height)
            LeadersTextLayout = new CanvasTextLayout(device, "ABCDEFG", LeadersFont, 0, 0);

            // calculate leaders position Y (X is calculated for each string)
            LeadersPosition = new Vector2(0, StringsPosition.Y);

            // leaders position is dynamic
            // strings position is dynamic

            NoLeadersTextLayout = new CanvasTextLayout(device, "No champions thusfar!", StringsFont, 0, 0);

            MaxY = (int)Position.Y + Height - Padding;
        }

        public override void Draw(CanvasAnimatedDrawEventArgs args)
        {
            base.Draw(args);

            float fCurrentY = StringsPosition.Y;

            // draw leaders
            //if (Leaders.Count == 0)
            //{
            //    args.DrawingSession.DrawTextLayout(NoLeadersTextLayout, StringsPosition, Colors.White);
            //}
            //else
            //{
            foreach (Leader leader in Leaders)
            {
                RichStringPart str = leader.ToRichString();
                args.DrawingSession.DrawText(str.String, new Vector2(LeadersPosition.X, fCurrentY), str.Color, LeadersFont);
                fCurrentY += (float)LeadersTextLayout.LayoutBounds.Height;
            }

            // draw first column of strings
            int i = 0;
            while ((fCurrentY + (float)StringsTextLayout.LayoutBounds.Height) < MaxY && i < Strings.Count)
            {
                RichStringPart str = Strings[i].ToRichString();
                args.DrawingSession.DrawText(str.String, new Vector2(StringsPosition.X, fCurrentY), str.Color, StringsFont);
                fCurrentY += (float)StringsTextLayout.LayoutBounds.Height;
                i++;
            }

            // set new x position as listboxposition.x + half width + padding
            float fCurrentX = Position.X + Width / 2 + Padding;
            fCurrentY = StringsPosition.Y;

            // draw second column of strings
            while (i < Strings.Count)
            {
                RichStringPart str = Strings[i].ToRichString();
                args.DrawingSession.DrawText(str.String, new Vector2(fCurrentX, fCurrentY), str.Color, StringsFont);
                fCurrentY += (float)StringsTextLayout.LayoutBounds.Height;
                i++;
            }
            //}
        }

        public void RecalculateLayout()
        {
            StringsPosition = new Vector2(Position.X + Padding, LeadersPosition.Y + (float)LeadersTextLayout.LayoutBounds.Height * Leaders.Count);
        }
    }
}
