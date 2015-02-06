using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreepScoreAPI;

namespace LoL.Model
{
    public class Region
    {
        public String Name { get; set; }
        public CreepScore.Region Code { get; set; }
    }
}
