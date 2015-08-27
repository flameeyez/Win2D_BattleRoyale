using Microsoft.Graphics.Canvas.Text;
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
    public class RichString : IRichString
    {
        private List<RichStringPart> Parts = new List<RichStringPart>();
        private CanvasTextFormat LastFont { get; set; }
        private List<float> Widths = new List<float>();
        public float Width
        {
            get
            {
                return Widths.Sum();
            }
        }

        public RichString(RichStringPart initialStringPart)
        {
            Parts.Add(initialStringPart);
        }

        public RichString(string strInitial)
        {
            Parts.Add(new RichStringPart(strInitial, Colors.White));
        }

        public void Add(RichStringPart partToAdd)
        {
            Parts.Add(partToAdd);
        }

        public void Draw(CanvasAnimatedDrawEventArgs args, Vector2 position, CanvasTextFormat font)
        {
            if (LastFont == null || LastFont.FontFamily != font.FontFamily || LastFont.FontSize != font.FontSize)
            {
                Widths.Clear();

                // recalculate widths
                for(int i = 0; i < Parts.Count; i++)
                {
                    CanvasTextLayout layoutTemp = new CanvasTextLayout(args.DrawingSession, Parts[i].String.Replace(' ', '.'), font, 0, 0);
                    Widths.Add((float)layoutTemp.LayoutBounds.Width);
                }

                LastFont = font;
            }

            float fOffsetX = 0.0f;
            for (int i = 0; i < Parts.Count; i++)
            {
                // draw string part
                args.DrawingSession.DrawText(Parts[i].String, new Vector2(position.X + fOffsetX, position.Y), Parts[i].Color, font);

                // add to offset
                fOffsetX += Widths[i];
            }
        }

        public RichString ToRichString()
        {
            return this;
        }
    }
}
