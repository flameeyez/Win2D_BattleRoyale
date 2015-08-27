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

        // subset of Strings (cast as Leaders, sorted on most wins)
        public List<Leader> Leaders = new List<Leader>();

        private int MaxY { get; set; }

        public RichListBoxLeaderboard(CanvasDevice device, Vector2 position, int width, int height, string title, CanvasTextFormat titleFont, CanvasTextFormat stringsFont, CanvasTextFormat leadersFont)
            : base(device, position, width, height, title, titleFont, stringsFont, 0)
        {
            LeadersFont = leadersFont;

            // calculate leaders text layout (for text height)
            LeadersTextLayout = new CanvasTextLayout(device, "ABCDEFG", LeadersFont, 0, 0);

            // calculate leaders position Y (X is calculated for each string)
            LeadersPosition = new Vector2(StringsPosition.X, StringsPosition.Y);

            // leaders position is dynamic
            // strings position is dynamic

            MaxY = (int)Position.Y + Height - Padding;
        }

        public override void Draw(CanvasAnimatedDrawEventArgs args)
        {
            base.Draw(args);

            // draw leaders
            int i = 0;
            for(i = 0; i < Leaders.Count; i++)
            {
                RichString str = Leaders[i].ToRichString();

                //CanvasTextLayout layout = new CanvasTextLayout(args.DrawingSession, str.String, LeadersFont, 0, 0);
                float x = Position.X;// + (Width - str.Width) / 2;

                str.Draw(args, 
                    new Vector2(x, LeadersPosition.Y + i * (float)LeadersTextLayout.LayoutBounds.Height), 
                    LeadersFont);
            }

            // draw first column of strings
            float fCurrentY = StringsPosition.Y;
            i = 0;
            while ((fCurrentY + (float)StringsTextLayout.LayoutBounds.Height) < MaxY && i < Strings.Count)
            {
                RichString str = Strings[i].ToRichString();
                str.Draw(args, new Vector2(StringsPosition.X, fCurrentY), StringsFont);
                fCurrentY += (float)StringsTextLayout.LayoutBounds.Height;
                i++;
            }

            // set second column x position
            float fCurrentX = Position.X + Width / 2 + Padding;
            fCurrentY = StringsPosition.Y;

            // draw second column of strings
            while (i < Strings.Count)
            {
                RichString str = Strings[i].ToRichString();
                str.Draw(args, new Vector2(fCurrentX, fCurrentY), StringsFont);
                fCurrentY += (float)StringsTextLayout.LayoutBounds.Height;
                i++;
            }
        }

        public void RecalculateLayout()
        {
            StringsPosition = new Vector2(Position.X + Padding, LeadersPosition.Y + (float)LeadersTextLayout.LayoutBounds.Height * Leaders.Count + Padding);
        }
    }
}
