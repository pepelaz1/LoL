﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreepScoreAPI;

namespace LoL.Model
{
    public class ChampData
    {
        public ChampionStatic Static {get;set;}
        public ChampionStats Stats { get; set; }
    }
}
