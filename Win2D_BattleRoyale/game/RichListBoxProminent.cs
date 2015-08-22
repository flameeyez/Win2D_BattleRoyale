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
        public bool ProminentString { get; set; }
        public int MaxStrings { get; set; }

        public CanvasTextFormat StringsFont { get; set; }
        public CanvasTextFormat ProminentStringFont { get; set; }
        private CanvasTextLayout ProminentStringLayout { get; set; }

        // used for title padding, strings padding, prominent last string padding
        public static int Padding = 10;

        public Vector2 BarUnderStringsLeft { get; set; }
        public Vector2 BarUnderStringsRight { get; set; }
        public Vector2 ProminentStringPosition { get; set; }

        public RichListBoxProminent(CanvasDevice device, Vector2 position, int width, string title, CanvasTextFormat titleFont, int maxStrings, CanvasTextFormat stringsFont, bool prominentString = false, CanvasTextFormat prominentStringFont = null, bool isTwoColumn = false) : base()
        {
            Position = position;
            ProminentString = prominentString;

            // title
            Title = new CanvasTextLayout(device, title, titleFont, 0, 0);
            TitlePosition = new Vector2(Position.X + Padding, Position.Y + Padding);

            // width is derived from title bounds
            Width = width;  //(int)Title.LayoutBounds.Width + Padding * 2;

            // bar under title
            BarUnderTitleLeft = new Vector2(Position.X, Position.Y + Padding * 2 + (float)Title.LayoutBounds.Height);
            BarUnderTitleRight = new Vector2(Position.X + Width, Position.Y + Padding * 2 + (float)Title.LayoutBounds.Height);

            // calculate strings y position
            StringsPosition = new Vector2(Position.X + Padding, BarUnderTitleRight.Y + Padding);

            // height of max strings
            MaxStrings = maxStrings;
            StringsFont = stringsFont;
            StringsTextLayout = new CanvasTextLayout(device, "THIS IS A PRETTY GOOD TEMP STRING", StringsFont, 0, 0);
            double dStringsHeight;
            double dTotalHeight;

            // calculate height
            // padding, title height, padding, padding, strings height, padding[, prominent string height, padding]
            if (ProminentString)
            {
                dStringsHeight = StringsTextLayout.LayoutBounds.Height * (MaxStrings - 1);

                ProminentStringLayout = new CanvasTextLayout(device, "THIS IS A PRETTY GOOD TEMP STRING", prominentStringFont, 0, 0);
                dTotalHeight = Title.LayoutBounds.Height + Padding * 6 + dStringsHeight + ProminentStringLayout.LayoutBounds.Height;

                ProminentStringFont = prominentStringFont;

                // bar under strings
                BarUnderStringsLeft = new Vector2(Position.X, StringsPosition.Y + (float)dStringsHeight + Padding);
                BarUnderStringsRight = new Vector2(Position.X + Width, StringsPosition.Y + (float)dStringsHeight + Padding);

                // prominent string
                ProminentStringPosition = new Vector2(Position.X + Padding, BarUnderStringsRight.Y + Padding);
            }
            else
            {
                dStringsHeight = StringsTextLayout.LayoutBounds.Height * MaxStrings;
                dTotalHeight = Title.LayoutBounds.Height + Padding * 4 + dStringsHeight;
            }

            Height = (int)dTotalHeight;

            // border
            BorderRectangle = new Rect(Position.X, Position.Y, Width, Height);

            //CanvasTextLayout winningTextLayout = new CanvasTextLayout(args.DrawingSession, strWinnerString, Statics.FontExtraLarge, 0.0f, 0.0f);
            //double dWinningStringWidth = winningTextLayout.DrawBounds.Width;
            //fWinnerStringPositionX = (float)((Statics.PixelScale * Statics.MapTilesX - dWinningStringWidth) / 2) + Statics.MapOffsetX;
            //fWinnerStringPositionY = 40.0f + Statics.MapOffsetY;
        }

        #region Draw/Update
        public virtual void Draw(CanvasAnimatedDrawEventArgs args)
        {
            // border
            args.DrawingSession.DrawRectangle(BorderRectangle, Colors.White);

            // title
            args.DrawingSession.DrawTextLayout(Title, TitlePosition, Colors.White);

            // bar under title
            args.DrawingSession.DrawLine(BarUnderTitleLeft, BarUnderTitleRight, Colors.White);

            // strings
            float fCurrentY = StringsPosition.Y;
            if (ProminentString)
            {
                for (int i = 0; i < Strings.Count - 1; i++)
                {
                    args.DrawingSession.DrawText(Strings[i].String, new Vector2(StringsPosition.X, fCurrentY), Strings[i].Color, StringsFont);
                    fCurrentY += (float)StringsTextLayout.LayoutBounds.Height;
                }

                // bar below strings
                args.DrawingSession.DrawLine(BarUnderStringsLeft, BarUnderStringsRight, Colors.White);

                if (Strings.Count > 0)
                {
                    // prominent string
                    args.DrawingSession.DrawText(Strings[Strings.Count - 1].String, ProminentStringPosition, Strings[Strings.Count - 1].Color, ProminentStringFont);

                    // debug
                    CanvasTextLayout layout = new CanvasTextLayout(args.DrawingSession.Device, Strings[Strings.Count - 1].String, ProminentStringFont, 0, 0);
                    Statics.MaxStringWidth = ((int)layout.LayoutBounds.Width > Statics.MaxStringWidth) ? (int)layout.LayoutBounds.Width : Statics.MaxStringWidth;
                }


            }
            else
            {
                for (int i = 0; i < Strings.Count; i++)
                {
                    args.DrawingSession.DrawText(Strings[i].String, new Vector2(StringsPosition.X, fCurrentY), Strings[i].Color, StringsFont);
                    fCurrentY += (float)StringsTextLayout.LayoutBounds.Height;
                }
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
