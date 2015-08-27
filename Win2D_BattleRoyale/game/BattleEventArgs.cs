using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win2D_BattleRoyale
{
    public class BattleEventArgs
    {
        public Region Winner { get; set; }
        public string DefeatString { get; set; }
        public Region Loser { get; set; }

        private int _overtakestep { get; set; }
        private static int _overtakeframes = 1;//50;
        public int OvertakeStep
        {
            get
            {
                return _overtakestep;
            }
        }

        public BattleEventArgs(Region winner, string strDefeatString, Region loser)
        {
            Winner = winner;
            DefeatString = strDefeatString;
            Loser = loser;

            _overtakestep = loser.Tiles.Count / _overtakeframes;
        }

        public override string ToString()
        {
            return Winner.Name + " " + DefeatString + " " + Loser.Name + "\n";
        }

        public RichString ToRichString()
        {
            RichString rs = new RichString(new RichStringPart(Winner.Name + " ", Winner.Color));
            rs.Add(new RichStringPart(DefeatString + " "));
            rs.Add(new RichStringPart(Loser.Name, Loser.Color));

            return rs;
        }
    }
}
