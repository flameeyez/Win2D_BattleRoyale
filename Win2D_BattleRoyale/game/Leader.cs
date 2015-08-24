using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Win2D_BattleRoyale
{
    public class Leader : IRichString
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return Title + " " + FirstName + " " + LastName;
            }
        }
        public int Wins { get; set; }

        public int BattleWins { get; set; }
        public int BattleLosses { get; set; }

        public Leader(string firstname, string lastname, string gender)
        {
            Wins = 0;

            BattleWins = 0;
            BattleLosses = 0;

            FirstName = firstname;
            LastName = lastname;
            
            switch(gender)
            {
                case "M":
                    Title = Statics.MaleTitles.RandomString();
                    break;
                case "F":
                    Title = Statics.FemaleTitles.RandomString();
                    break;
            }            
        }

        public override string ToString()
        {
            return FullName;
        }

        public string ToLeaderboardString()
        {
            return ToString() + ": " + Wins.ToString() + " (" + BattleWins.ToString() + "-" + BattleLosses.ToString() + ", " + ((double)BattleWins / (BattleWins + BattleLosses)).ToString("F3") + ")";
        }

        public RichStringPart ToRichString()
        {
            return new RichStringPart(ToLeaderboardString(), Colors.White);
        }
    }
}
