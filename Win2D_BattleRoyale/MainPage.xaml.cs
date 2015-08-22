using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Numerics;
using Windows.Foundation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Win2D_BattleRoyale
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Map map;
        RichListBoxProminent rlb;

        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
        }

        #region Initialization
        private void canvasMain_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }
        async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            // c.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 200);
            // c.Paused = true;

            Statics.CrownImage = await CanvasBitmap.LoadAsync(sender, "game\\crown.png");
            Statics.LeftColumnWidth = (int)sender.Size.Width - Statics.RightColumnWidth - Statics.RightColumnPadding * 2 - Statics.LeftColumnPadding * 2;
            Statics.CanvasHeight = (int)sender.Size.Height;

            Statics.ColumnDividerTop = new Vector2(Statics.LeftColumnWidth + Statics.LeftColumnPadding * 2, 0);
            Statics.ColumnDividerBottom = new Vector2(Statics.LeftColumnWidth + Statics.LeftColumnPadding * 2, (float)sender.Size.Height);

            rlb = new RichListBoxProminent(sender.Device, 
                                           new Vector2(Statics.LeftColumnWidth + Statics.LeftColumnPadding * 2 + Statics.RightColumnPadding, Statics.RightColumnPadding),
                                           Statics.RightColumnWidth, 
                                           "Census of Recent Contentions", Statics.FontLarge, 
                                           20, Statics.FontSmall, 
                                           Statics.FontLarge);

            Leaderboard.Position = new Vector2(rlb.Position.X, rlb.Position.Y + rlb.Height + Statics.RightColumnPadding);
            Leaderboard.Width = rlb.Width;
            Leaderboard.Height = (int)sender.Size.Height - rlb.Height - Statics.RightColumnPadding * 3;

            // resets map, listbox, debug frame count
            Reset(sender);
        }
        private void Reset(ICanvasAnimatedControl sender)
        {
            Statics.FrameCount = 0;
            rlb.Clear();
            map = new Map(Statics.MapPosition);

            //rlb.Add(new RichStringPart("DEBUG 1", Colors.Red));
            //rlb.Add(new RichStringPart("DEBUG 2", Colors.Orange));
            //rlb.Add(new RichStringPart("DEBUG 3", Colors.Yellow));
            //rlb.Add(new RichStringPart("DEBUG 4", Colors.Green));
            //rlb.Add(new RichStringPart("DEBUG 5", Colors.Blue));
            //rlb.Add(new RichStringPart("DEBUG 6", Colors.Indigo));
            //rlb.Add(new RichStringPart("DEBUG 7", Colors.Violet));
            //rlb.Add(new RichStringPart("DEBUG 8", Colors.Brown));
            //rlb.Add(new RichStringPart("DEBUG 9", Colors.Pink));
            //rlb.Add(new RichStringPart("DEBUG 10", Colors.Turquoise));
        }
        #endregion

        #region Draw
        private void canvasMain_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            map.Draw(args);
            rlb.Draw(args);
            // args.DrawingSession.DrawLine(Statics.ColumnDividerTop, Statics.ColumnDividerBottom, Colors.White);
            Leaderboard.Draw(args);

            DrawDebug(args);
        }
        private void DrawDebug(CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawText("Mouse: " + Statics.MouseX.ToString() + ", " + Statics.MouseY.ToString(), new Vector2(1200, 800), Colors.White);
            args.DrawingSession.DrawText("Max String: " + Statics.MaxStringWidth.ToString(), new Vector2(1200, 820), Colors.White);

            //args.DrawingSession.DrawText("Frame Counter: " + nFrameCount.ToString(), new System.Numerics.Vector2(1000, 10), Colors.White);
            //args.DrawingSession.DrawText("Frame Time: " + args.Timing.ElapsedTime.TotalMilliseconds.ToString() + "ms", new System.Numerics.Vector2(1000, 30), Colors.White);
        }
        #endregion

        #region Update
        private void canvasMain_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            if (map.Finished)
            {
                Reset(sender);
            }
            else
            {
                Statics.FrameCount++;
                map.Update(rlb, args);
            }
        }
        #endregion

        #region Input
        private void Grid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            canvasMain.Paused = !canvasMain.Paused;
        }
        private void gridMain_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Statics.MouseX = (int)e.GetCurrentPoint(gridMain).Position.X;
            Statics.MouseY = (int)e.GetCurrentPoint(gridMain).Position.Y;
        }
        #endregion
    }
}