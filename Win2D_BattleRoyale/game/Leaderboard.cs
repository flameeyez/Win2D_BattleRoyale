using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace Win2D_BattleRoyale
{
    public static class Leaderboard
    {
        public static List<Leader> Leaders = new List<Leader>();

        public static void DeclareWinner(string strWinner)
        {
            foreach (Leader leader in Leaders)
            {
                if (leader.FullName == strWinner)
                {
                    leader.Wins++;

                    Leader match = Leaders.Find(x => x.FullName == strWinner);
                    if (match == null)
                    {
                        Leaders.Add(leader);
                    }

                    Leaders.Sort((x, y) => y.Wins.CompareTo(x.Wins));
                    break;
                }
            }
        }

        static Leaderboard()
        {
            Leaders.Add(new Leader("Ken", "Pacquer", "M"));
            Leaders.Add(new Leader("Joshua", "Baxter", "M"));
            Leaders.Add(new Leader("Brian", "Lich", "M"));
            Leaders.Add(new Leader("Dan", "Simpson", "M"));
            Leaders.Add(new Leader("Pablo", "Ang", "M"));
            Leaders.Add(new Leader("Robert", "Hoover", "M"));
            Leaders.Add(new Leader("Alan", "Theurer", "M"));
            Leaders.Add(new Leader("Justin", "Hall", "M"));
            Leaders.Add(new Leader("Eliot", "Graff", "M"));
            Leaders.Add(new Leader("Don", "Marshall", "M"));
            Leaders.Add(new Leader("Eric", "Doku", "M"));
            Leaders.Add(new Leader("Nathan", "Bazan", "M"));
            Leaders.Add(new Leader("Court", "Faw", "M"));
            Leaders.Add(new Leader("Ted", "Hudek", "M"));
            Leaders.Add(new Leader("Barry", "Golden", "M"));

            Leaders.Add(new Leader("Melissa", "Johnson", "F"));
            Leaders.Add(new Leader("Janet", "Thomas", "F"));
            Leaders.Add(new Leader("Jeanie", "Decker", "F"));
            Leaders.Add(new Leader("Liz", "Ross", "F"));
            Leaders.Add(new Leader("Trudy", "Hakala", "F"));
            Leaders.Add(new Leader("Priyanka", "Wilkins", "F"));
            Leaders.Add(new Leader("Celeste", "de Guzman", "F"));
            Leaders.Add(new Leader("Maricia", "Alforque", "F"));
            Leaders.Add(new Leader("Kathy", "Narvaez", "F"));
            Leaders.Add(new Leader("Angie", "Nielsen", "F"));
            Leaders.Add(new Leader("Dawn", "Wood", "F"));
        }

        //    public static void Draw(CanvasAnimatedDrawEventArgs args)
        //    {
        //        args.DrawingSession.DrawRectangle(new Rect(Position.X, Position.Y, Width, Height), Colors.White);

        //        //args.DrawingSession.DrawText("Champions of the Realm", new Vector2(Position.X, 413), Colors.White, Statics.FontLarge);

        //        //float fCurrentX = Position.X;
        //        //float fCurrentY = Position.Y;

        //        //int i = 0;
        //        //while (Leaders[i].Wins == Leaders[0].Wins && Leaders[0].Wins > 0)
        //        //{
        //        //    args.DrawingSession.DrawText(Leaders[i].ToLeaderboardString(), new Vector2(fCurrentX + 200, fCurrentY), Colors.White, Statics.FontMedium);
        //        //    fCurrentY += 27;
        //        //    i++;
        //        //}

        //        //if (i > 0) { fCurrentY += 20; }
        //        //float fBelowLeadersY = fCurrentY;

        //        //while (fCurrentY < 940 && i < Leaders.Count)
        //        //{
        //        //    args.DrawingSession.DrawText(Leaders[i].ToLeaderboardString(), new Vector2(fCurrentX, fCurrentY), Colors.White, Statics.FontSmall);
        //        //    fCurrentY += 20;
        //        //    i++;
        //        //}

        //        //fCurrentX += 450;
        //        //fCurrentY = fBelowLeadersY;

        //        //while (i < Leaders.Count)
        //        //{
        //        //    args.DrawingSession.DrawText(Leaders[i].ToLeaderboardString(), new Vector2(fCurrentX, fCurrentY), Colors.White, Statics.FontSmall);
        //        //    fCurrentY += 20;
        //        //    i++;
        //        //}
        //    }
    }
}
