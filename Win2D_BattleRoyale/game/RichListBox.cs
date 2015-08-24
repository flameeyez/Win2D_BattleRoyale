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
    public abstract class RichListBox
    {
        #region Layout
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Position { get; set; }
        #endregion

        #region Title
        public CanvasTextLayout Title { get; set; }
        public CanvasTextFormat TitleFont { get; set; }
        public Vector2 TitlePosition { get; set; }
        #endregion

        #region Main Content
        protected List<IRichString> Strings = new List<IRichString>();
        protected CanvasTextLayout StringsTextLayout { get; set; }
        public CanvasTextFormat StringsFont { get; set; }
        public Vector2 StringsPosition { get; set; }

        public int MaxStrings { get; set; }
        #endregion

        #region Borders
        public Rect BorderRectangle { get; set; }
        public Vector2 BarUnderTitleLeft { get; set; }
        public Vector2 BarUnderTitleRight { get; set; }
        #endregion

        public static int Padding = 10;

        public RichListBox(CanvasDevice device, Vector2 position, int width, int height, string title, CanvasTextFormat titleFont, CanvasTextFormat stringsFont, int maxStrings = 0)
        {
            // base(device, position, width, 0, title, titleFont)
            Position = position;

            // title
            Title = new CanvasTextLayout(device, title, titleFont, 0, 0);
            TitlePosition = new Vector2(Position.X + Padding, Position.Y + Padding);

            // width is derived from title bounds
            Width = width;  //(int)Title.LayoutBounds.Width + Padding * 2;
            Height = height;

            // bar under title
            BarUnderTitleLeft = new Vector2(Position.X, Position.Y + Padding * 2 + (float)Title.LayoutBounds.Height);
            BarUnderTitleRight = new Vector2(Position.X + Width, Position.Y + Padding * 2 + (float)Title.LayoutBounds.Height);

            // strings
            StringsFont = stringsFont;
            StringsTextLayout = new CanvasTextLayout(device, "THIS IS A PRETTY GOOD TEMP STRING", StringsFont, 0, 0);
            StringsPosition = new Vector2(Position.X + Padding, BarUnderTitleRight.Y + Padding);

            MaxStrings = maxStrings == 0 ? 1000 : maxStrings;

            BorderRectangle = new Rect(Position.X, Position.Y, Width, Height);
        }

        public virtual void Draw(CanvasAnimatedDrawEventArgs args)
        {
            // border
            args.DrawingSession.DrawRectangle(BorderRectangle, Colors.White);

            // title
            args.DrawingSession.DrawTextLayout(Title, TitlePosition, Colors.White);

            // bar under title
            args.DrawingSession.DrawLine(BarUnderTitleLeft, BarUnderTitleRight, Colors.White);
        }
    }
}
