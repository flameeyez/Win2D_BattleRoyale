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
        private List<Leader> Leaders = new List<Leader>();

        private int MaxY { get; set; }

        public RichListBoxLeaderboard(CanvasDevice device, Vector2 position, int width, int height, string title, CanvasTextFormat titleFont, CanvasTextFormat stringsFont, CanvasTextFormat leadersFont)
            : base(device, position, width, height, title, titleFont, stringsFont, 0)
        {
            RecalculateLeaders();

            LeadersFont = leadersFont;

            // calculate leaders text layout (for text height)
            LeadersTextLayout = new CanvasTextLayout(device, "ABCDEFG", LeadersFont, 0, 0);

            // calculate leaders position Y
            LeadersPosition = new Vector2(0, StringsPosition.Y);

            // leaders position is dynamic
            // strings position is dynamic

            NoLeadersTextLayout = new CanvasTextLayout(device, "No champions thusfar!", StringsFont, 0, 0);
        }

        public override void Draw(CanvasAnimatedDrawEventArgs args)
        {
            base.Draw(args);

            // draw leaders
            if (Leaders.Count == 0)
            {
                args.DrawingSession.DrawTextLayout(NoLeadersTextLayout, StringsPosition, Colors.White);
            }
            else
            {
                foreach (Leader leader in Leaders)
                {

                }
            }

            // draw first column of strings

            // draw second column of strings
        }

        private void RecalculateLeaders()
        {
            Strings.Clear();
            Leaders.Clear();

            // leaderboard is sorted
            Leaders.Add(Leaderboard.Leaders[0]);

            int i = 1;
            for(i = 1; i < Leaderboard.Leaders.Count; i++)
            {
                if(Leaderboard.Leaders[i].Wins == Leaderboard.Leaders[0].Wins)
                {
                    Leaders.Add(Leaderboard.Leaders[i]);
                }
                else
                {
                    break;
                }
            }

            while(i < Leaderboard.Leaders.Count)
            {
                Strings.Add(Leaderboard.Leaders[i]);
            }
        }
    }
}
