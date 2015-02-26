using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CreepScoreAPI;

namespace LoL.Model
{
    public class SummonerData
    {

        public Summoner Summoner { get; set; }
        public RankedStats RankedStats { get; set; }
        public ChampionStats Ranked { get; set; }
        public bool RankedFound { get; set; }
        public bool NormalFound { get; set; }
        public String KillsRanked { get; set; }
        public String KillsNormal { get; set; }
        public String AssistsRanked { get; set; }
        public String AssistsNormal { get; set; }
        public String DeathsRanked { get; set; }
        public String GoldEarned { get; set; }
        public String MinionKills { get; set; }
        public String WinsNormal { get; set; }
        public String TotalHoursPlayed { get; set; }// in hours
        public String TotalDaysPlayed { get; set; }// in days
        public String PentaKills { get; set; }
        public String QuadraKills { get; set; }
        public String TripleKills { get; set; }
        public String DoubleKills { get; set; }
        public String Warding { get; set; }
        public String WardScore { get; set; }
        public String WardMage { get; set; }
        public String WardAssasin { get; set; }
        public String WardMarksman { get; set; }
        public String WardFighter { get; set; }
        public String WardTank { get; set; }
        public String WardSupport { get; set; }
        public ImageSource Team3v3Image { get; set; }
        public String Team3v3Rating { get; set; }
        public String Team3v3LeaguePoints { get; set; }
        public String Team3v3Wins { get; set; }
        public String Team3v3Losses { get; set; }
        public ImageSource Solo5v5Image { get; set; }
        public String Solo5v5Rating { get; set; }
        public String Solo5v5LeaguePoints { get; set; }
        public String Solo5v5Wins { get; set; }
        public String Solo5v5Losses { get; set; }
         public ImageSource Team5v5Image { get; set; }
        public String Team5v5Rating { get; set; }
        public String Team5v5LeaguePoints { get; set; }
        public String Team5v5Wins { get; set; }
        public String Team5v5Losses { get; set; }

    }
}
