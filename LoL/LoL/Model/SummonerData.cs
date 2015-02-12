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

        public String TotalTimePlayed { get; set; }// in hours

    }
}
