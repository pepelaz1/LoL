using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreepScoreAPI;

namespace LoL.Model
{
    public class SummonerData
    {
        public PlayerStatsSummary Normal { get; set; } 

        public ChampionStats Ranked { get; set; }

        public String TotalHoursPlayed { get; set; }// in hours

        public String TotalDaysPlayed { get; set; }// in days

        public String WardScore { get; set; }
        public String WardMage { get; set; }
        public String WardAssasin { get; set; }
        public String WardMarksman { get; set; }
        public String WardFighter { get; set; }
        public String WardTank { get; set; }
        public String WardSupport { get; set; }
    }
}
