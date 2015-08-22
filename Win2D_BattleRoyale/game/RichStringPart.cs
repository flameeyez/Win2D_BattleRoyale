﻿using Windows.UI;

namespace Win2D_BattleRoyale
{
    public class RichStringPart
    {
        public string String { get; set; }
        public Color Color { get; set; }

        public RichStringPart(string str, Color color)
        {
            String = str;
            Color = color;
        }
    }
}