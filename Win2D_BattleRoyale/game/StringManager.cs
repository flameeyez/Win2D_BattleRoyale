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
    public static class StringManager
    {
        private static Vector2 Position = new Vector2(1000, 120);
        private static List<RichStringPart> Strings = new List<RichStringPart>();

        static StringManager()
        {
            Strings.Add(new RichStringPart("DEBUG FIRST STRING", Colors.Black));
        }

        public static void Add(string str)
        {
            Strings.Add(new RichStringPart(str, Colors.White));
            if (Strings.Count > 10) { Strings.RemoveAt(0); }
        }

        public static void Add(RichStringPart str)
        {
            Strings.Add(str);
            if (Strings.Count > 10) { Strings.RemoveAt(0); }
        }

        public static void Draw(CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawRectangle(new Rect(980, 50, 850, 50), Colors.White);
            args.DrawingSession.DrawRectangle(new Rect(980, 100, 850, 220), Colors.White);
            args.DrawingSession.DrawRectangle(new Rect(980, 320, 850, 50), Colors.White);

            args.DrawingSession.DrawText("Recent Contentions", new Vector2(Position.X, 53), Colors.White, Statics.FontLarge);

            float fCurrentY = Position.Y;
            int i = 0;
            for (i = 0; i < Strings.Count - 1; i++)
            {
                args.DrawingSession.DrawText(Strings[i].String, new Vector2(Position.X, fCurrentY), Strings[i].Color, Statics.FontSmall);
                fCurrentY += 20;
            }

            if (Strings.Count > 0)
            {
                args.DrawingSession.DrawText(Strings[i].String, new Vector2(Position.X, Position.Y + 205), Strings[i].Color, Statics.FontLarge);
            }
        }

        public static void Clear()
        {
            Strings.Clear();
        }
    }
}
