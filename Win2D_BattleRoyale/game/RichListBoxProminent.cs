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
    public class RichListBoxProminent : RichListBox
    {
        private Vector2 BarUnderStringsLeft { get; set; }
        private Vector2 BarUnderStringsRight { get; set; }

        private Vector2 ProminentStringPosition { get; set; }
        private CanvasTextFormat ProminentStringFont { get; set; }
        private CanvasTextLayout ProminentStringLayout { get; set; }

        public RichListBoxProminent(CanvasDevice device, Vector2 position, int width, string title, CanvasTextFormat titleFont, int maxStrings, CanvasTextFormat stringsFont, CanvasTextFormat prominentStringFont)
            : base(device, position, width, 0, title, titleFont, stringsFont, maxStrings)
        {
            // strings height
            double dStringsHeight = StringsTextLayout.LayoutBounds.Height * (MaxStrings - 1);

            // bar under strings
            BarUnderStringsLeft = new Vector2(Position.X, StringsPosition.Y + (float)dStringsHeight + Padding);
            BarUnderStringsRight = new Vector2(Position.X + Width, StringsPosition.Y + (float)dStringsHeight + Padding);

            // prominent string
            ProminentStringPosition = new Vector2(Position.X + Padding, BarUnderStringsRight.Y + Padding);
            ProminentStringFont = prominentStringFont;
            ProminentStringLayout = new CanvasTextLayout(device, "THIS IS A PRETTY GOOD TEMP STRING", ProminentStringFont, 0, 0);

            // total height
            Height = (int)(Title.LayoutBounds.Height + Padding * 6 + dStringsHeight + ProminentStringLayout.LayoutBounds.Height);

            // border
            BorderRectangle = new Rect(Position.X, Position.Y, Width, Height);
        }

        #region Draw/Update
        public override void Draw(CanvasAnimatedDrawEventArgs args)
        {
            base.Draw(args);

            // strings
            float fCurrentY = StringsPosition.Y;
            for (int i = 0; i < Strings.Count - 1; i++)
            {
                RichStringPart str = Strings[i].ToRichString();
                args.DrawingSession.DrawText(str.String, new Vector2(StringsPosition.X, fCurrentY), str.Color, StringsFont);
                fCurrentY += (float)StringsTextLayout.LayoutBounds.Height;
            }

            // bar below strings
            args.DrawingSession.DrawLine(BarUnderStringsLeft, BarUnderStringsRight, Colors.White);

            if (Strings.Count > 0)
            {
                RichStringPart str = Strings[Strings.Count - 1].ToRichString();

                // prominent string
                args.DrawingSession.DrawText(str.String, ProminentStringPosition, str.Color, ProminentStringFont);

                // debug
                CanvasTextLayout layout = new CanvasTextLayout(args.DrawingSession.Device, str.String, ProminentStringFont, 0, 0);
                Statics.MaxStringWidth = ((int)layout.LayoutBounds.Width > Statics.MaxStringWidth) ? (int)layout.LayoutBounds.Width : Statics.MaxStringWidth;
            }
        }
        public void Update(CanvasAnimatedUpdateEventArgs args)
        {

        }
        #endregion

        #region Add/Remove
        public void Add(RichStringPart str)
        {
            if (str == null) { return; }

            Strings.Add(str);
            if (Strings.Count > MaxStrings)
            {
                Strings.RemoveAt(0);
            }
        }
        public bool Remove(RichStringPart str)
        {
            return Strings.Remove(str);
        }
        public void RemoveAt(int nIndex)
        {
            Strings.RemoveAt(nIndex);
        }
        public void Clear()
        {
            Strings.Clear();
        }
        #endregion
    }
}
