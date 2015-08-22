using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win2D_BattleRoyale
{
    public class RichString
    {
        private List<RichStringPart> Parts = new List<RichStringPart>();
        private int nCurrentOffset = 0;

        public RichString(RichStringPart initialStringPart)
        {
            Parts.Add(initialStringPart);

            // add offset
        }

        public void Add(RichStringPart partToAdd)
        {
            Parts.Add(partToAdd);
        }

        public void Draw(CanvasAnimatedDrawEventArgs args)
        {

        }
    }
}
