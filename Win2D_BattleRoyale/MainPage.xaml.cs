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
        RichListBoxProminent rlbProminent;
        RichListBoxLeaderboard rlbLeaderboard;

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

            rlbProminent = new RichListBoxProminent(sender.Device, 
                                           new Vector2(Statics.LeftColumnWidth + Statics.LeftColumnPadding * 2 + Statics.RightColumnPadding, Statics.RightColumnPadding),
                                           Statics.RightColumnWidth, 
                                           "Census of Recent Contentions", Statics.FontLarge, 
                                           20, Statics.FontSmall, 
                                           Statics.FontLarge);

            rlbLeaderboard = new RichListBoxLeaderboard(sender.Device,
                                                        new Vector2(rlbProminent.Position.X, rlbProminent.Position.Y + rlbProminent.Height + Statics.RightColumnPadding),
                                                        rlbProminent.Width,
                                                        (int)sender.Size.Height - rlbProminent.Height - Statics.RightColumnPadding * 3,
                                                        "Champions of the Realm",
                                                        Statics.FontLarge,
                                                        Statics.FontSmall,
                                                        Statics.FontMedium);

            //Leaderboard.Position = new Vector2(rlb.Position.X, rlb.Position.Y + rlb.Height + Statics.RightColumnPadding);
            //Leaderboard.Width = rlb.Width;
            //Leaderboard.Height = (int)sender.Size.Height - rlb.Height - Statics.RightColumnPadding * 3;

            // resets map, listbox, debug frame count
            Reset(sender);
        }
        private void Reset(ICanvasAnimatedControl sender)
        {
            Statics.FrameCount = 0;
            rlbProminent.Clear();
            map = new Map(Statics.MapPosition);
        }
        #endregion

        #region Draw
        private void canvasMain_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            map.Draw(args);
            rlbProminent.Draw(args);
            rlbLeaderboard.Draw(args);

            // args.DrawingSession.DrawLine(Statics.ColumnDividerTop, Statics.ColumnDividerBottom, Colors.White);
            // Leaderboard.Draw(args);

            DrawDebug(args);
        }
        private void DrawDebug(CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawText("Mouse: " + Statics.MouseX.ToString() + ", " + Statics.MouseY.ToString(), new Vector2(1200, 800), Colors.White);
            args.DrawingSession.DrawText("Max String: " + Statics.MaxStringWidth.ToString(), new Vector2(1200, 820), Colors.White);
        }
        #endregion

        #region Update
        private void canvasMain_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            if (map.Finished)
            {
                // TODO: more separation of Leaderboard and RichListBoxLeaderboard
                Leaderboard.DeclareWinner(map.Regions[0].Leader.FullName);
                Reset(sender);
            }
            else
            {
                Statics.FrameCount++;
                map.Update(rlbProminent, args);
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