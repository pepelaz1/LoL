using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CreepScoreAPI;

namespace LoL.Model
{
    public class ChampData
    {
        public ChampionStatic Static {get;set;}
        public ChampionStats Stats { get; set; }
        public String Name { get; set; }
        public BitmapImage Picture { get; set; }
        public String Kills { get; set; }
        public String Deaths { get; set; }
        public String Assists { get; set; }
        public String Wins { get; set; }
        public String Losses { get; set; }

        public bool Found { get; set; }
    }
}
