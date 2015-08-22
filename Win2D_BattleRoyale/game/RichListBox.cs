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
        protected List<RichStringPart> Strings = new List<RichStringPart>();
        protected CanvasTextLayout StringsTextLayout { get; set; }
        public Vector2 StringsPosition { get; set; }
        #endregion

        #region Borders
        public Rect BorderRectangle { get; set; }
        public Vector2 BarUnderTitleLeft { get; set; }
        public Vector2 BarUnderTitleRight { get; set; }
        #endregion

        public RichListBox()
        {

        }

        public abstract void Draw(CanvasAnimatedDrawEventArgs args);
    }
}
