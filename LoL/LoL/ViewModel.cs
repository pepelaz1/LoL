using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CreepScoreAPI;
using CreepScoreAPI.Constants;
using LoL.Model;


namespace LoL
{
    public class ViewModel : INotifyPropertyChanged
    {
        // RiotApi C# Wrapper 
       // private RiotApi _api;

        // Creep.Score api wrapper
        private CreepScore _api;

        // Riot api key generated at https://developer.riotgames.com/ (user - pepelaz1) and it's limits
       
        // My key
        //private const String _apiKey = "1502847e-8f39-4fe7-a338-1c0895692085";
        
        // Pat's key
        private const String _apiKey = "84185186-2f61-402e-a832-126172fd0234";
        private const int _limit_per_10s = 10;
        private const int _limit_per_10m = 500;

        // Current summoner
        private Summoner _summoner = null;

   
        // List of regions
        private List<Region> _regions = null;
        public List<Region> Regions
        {
            get
            {
                if (_regions == null)
                {
                    // Populate list of regions
                    _regions = new List<Region>();
                    Region r = new Region() { Name = "North America", Code = CreepScore.Region.NA };
                    _selectedRegion = r;
                    _regions.Add(r);
                    _regions.Add(new Region() { Name = "Europe West", Code = CreepScore.Region.EUW });
                    _regions.Add(new Region() { Name = "Europe Nordic & East", Code = CreepScore.Region.EUNE });
                    _regions.Add(new Region() { Name = "Brazil", Code = CreepScore.Region.BR });
                    _regions.Add(new Region() { Name = "Turkey", Code = CreepScore.Region.TR });
                    _regions.Add(new Region() { Name = "Russia", Code = CreepScore.Region.RU });
                    _regions.Add(new Region() { Name = "Latin Ameria North", Code = CreepScore.Region.LAN });
                    _regions.Add(new Region() { Name = "Latin Ameria South", Code = CreepScore.Region.LAS });
                    _regions.Add(new Region() { Name = "Oceania", Code = CreepScore.Region.OCE });
                    OnPropertyChanged("SelectedRegion");

                }
                return _regions;
            }
        }

        // Currently selected region
        private Region _selectedRegion;
        public Region SelectedRegion 
        {
            get
            {
                return _selectedRegion;
            }
            set
            {
                _selectedRegion = value;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }



        private String _summoner_name = "Summoner Name...";
        public String SummonerName
        {
            get
            {
                return _summoner_name;
            }
            set
            {
                _summoner_name = value;
                OnPropertyChanged("SummonerName");            
            }
        }

        public Visibility SummonerInfoVisibility
        {
            get
            {
                return (_summoner == null) ? Visibility.Hidden : Visibility.Visible;
            }
        }

        /// <summary>
        /// Property returning received summoner name
        /// </summary>
        public String SummonerTitle
        {
            get
            {
                return (_summoner == null) ? "" : _summoner.name;
            }
        }

        /// Property returning received summoner level
        public String SummonerLevel
        {
            get
            {
                return (_summoner == null) ? "" : _summoner.summonerLevel.ToString();
            }
        }

        public bool TabPagesEnabled 
        {
            get {
                
                return _summoner != null;
                //return true;
            }
        }

        
        ///
        /// Champions data
        ///
        // Current summoner ranked stats
        private RankedStats _rankedStats = null;

        // Current summoner's champion data
        private List<ChampData> _champData = new List<ChampData>();

        // Top 5 champs names
        public String Champ1Name 
        {
            get { return _champData.Count() > 0 ? _champData[0].Static.name : ""; }
        }

        public String Champ2Name
        {
            get { return _champData.Count() > 1 ? _champData[1].Static.name : ""; }
        }

        public String Champ3Name
        {
            get { return _champData.Count() > 2 ? _champData[2].Static.name : ""; }
        }

        public String Champ4Name
        {
            get { return _champData.Count() > 3 ? _champData[3].Static.name : ""; }
        }

        public String Champ5Name
        {
            get { return _champData.Count() > 4 ? _champData[4].Static.name : ""; }
        }

        // Top 5 champs total games count
        public String Champ1Games
        {
            get { return _champData.Count() > 0 ? _champData[0].Stats.stats.totalSessionsPlayed.ToString() : ""; }
        }

        public String Champ2Games
        {
            get { return _champData.Count() > 1 ? _champData[1].Stats.stats.totalSessionsPlayed.ToString() : ""; }
        }

        public String Champ3Games
        {
            get { return _champData.Count() > 2 ? _champData[2].Stats.stats.totalSessionsPlayed.ToString() : ""; }
        }

        public String Champ4Games
        {
            get { return _champData.Count() > 3 ? _champData[3].Stats.stats.totalSessionsPlayed.ToString() : ""; }
        }

        public String Champ5Games
        {
            get { return _champData.Count() > 4 ? _champData[4].Stats.stats.totalSessionsPlayed.ToString() : ""; }
        }

        // Top 5 champs total wins count
        public String Champ1Wins
        {
            get { return _champData.Count() > 0 ? _champData[0].Stats.stats.totalSessionsWon.ToString() : ""; }
        }

        public String Champ2Wins
        {
            get { return _champData.Count() > 1 ? _champData[1].Stats.stats.totalSessionsWon.ToString() : ""; }
        }

        public String Champ3Wins
        {
            get { return _champData.Count() > 2 ? _champData[2].Stats.stats.totalSessionsWon.ToString() : ""; }
        }

        public String Champ4Wins
        {
            get { return _champData.Count() > 3 ? _champData[3].Stats.stats.totalSessionsWon.ToString() : ""; }
        }

        public String Champ5Wins
        {
            get { return _champData.Count() > 4 ? _champData[4].Stats.stats.totalSessionsWon.ToString() : ""; }
        }

        // Top 5 champs total looses count
        public String Champ1Looses
        {
            get { return _champData.Count() > 0 ? _champData[0].Stats.stats.totalSessionsLost.ToString() : ""; }
        }

        public String Champ2Looses
        {
            get { return _champData.Count() > 1 ? _champData[1].Stats.stats.totalSessionsLost.ToString() : ""; }
        }

        public String Champ3Looses
        {
            get { return _champData.Count() > 2 ? _champData[2].Stats.stats.totalSessionsLost.ToString() : ""; }
        }

        public String Champ4Looses
        {
            get { return _champData.Count() > 3 ? _champData[3].Stats.stats.totalSessionsLost.ToString() : ""; }
        }

        public String Champ5Looses
        {
            get { return _champData.Count() > 4 ? _champData[4].Stats.stats.totalSessionsLost.ToString() : ""; }
        }


        ///
        /// Summoner data
        /// 
        private SummonerData _summonerData = new SummonerData();

        // Ranked 5v5 stat
        public String Kills
        {
            get { return _summonerData.Ranked.stats.totalChampionKills.ToString(); }
        }

        public String Deaths
        {
            get { return _summonerData.Ranked.stats.totalDeathsPerSession.ToString(); }
        }

        public String DoubleKills
        {
            get { return _summonerData.Ranked.stats.totalDoubleKills.ToString(); }
        }

        public String TripleKills
        {
            get { return _summonerData.Ranked.stats.totalTripleKills.ToString(); }
        }

        public String QuadraKills
        {
            get { return _summonerData.Ranked.stats.totalQuadraKills.ToString(); }
        }

        public String PentaKills
        {
            get { return _summonerData.Ranked.stats.totalPentaKills.ToString(); }
        }

        public String WinsRanked
        {
            get { return _summonerData.Ranked.stats.totalSessionsWon.ToString(); }
        }

        public String LossesRanked
        {
            get { return _summonerData.Ranked.stats.totalSessionsLost.ToString(); }
        }

        public String AssistsRanked
        {
            get { return _summonerData.Ranked.stats.totalAssists.ToString(); }
        }

        // Normal 5v5 stat
        public String WinsNormal
        {
            get { return _summonerData.Normal.wins.ToString(); }
        }

        public String LossesNormal
        {
            get { return _summonerData.Normal.losses.ToString(); }
        }

        public String AssistsNormal
        {
            get { return _summonerData.Normal.aggregatedStats.totalAssists.ToString(); }
        }

        ///
        /// Career averages
        /// 

        public String AverageKills
        {
            get { return (_summonerData.Ranked.stats.totalChampionKills / _summonerData.Ranked.stats.totalSessionsPlayed).ToString(); }
        }

        public String AverageDeaths
        {
            get { return (_summonerData.Ranked.stats.totalDeathsPerSession / _summonerData.Ranked.stats.totalSessionsPlayed).ToString(); }
        }

        public String AverageGold
        {
            get { return (_summonerData.Ranked.stats.totalGoldEarned / _summonerData.Ranked.stats.totalSessionsPlayed).ToString(); }
        }

        public String AverageFarm
        {
            get { return (_summonerData.Ranked.stats.totalMinionKills / _summonerData.Ranked.stats.totalSessionsPlayed).ToString(); }
        }
        public String TotalGames
        {
            get { return _summonerData.Ranked.stats.totalSessionsPlayed.ToString(); }
        }

            /// <summary>
        /// View model constructor
        /// </summary>
        public ViewModel()
        {
             _api = new CreepScore(_apiKey, _limit_per_10s, _limit_per_10m);
        }

        public async Task QueryData()
        {                  
            _summoner = null;
            _summoner = await _api.RetrieveSummoner(SelectedRegion.Code, SummonerName);
            if (_summoner == null)
                throw new Exception("Summoner not found");

            _rankedStats = await _summoner.RetrieveRankedStats(CreepScore.Season.Season2015);
            _champData.Clear();
            
     
            foreach (var o in (from x in _rankedStats.champions
                               where x.id != 0
                               orderby x.stats.totalSessionsWon descending, x.stats.totalSessionsPlayed descending
                               select x).Take(5))
            {
                var static1 = await _api.RetrieveChampionData(SelectedRegion.Code, o.id, StaticDataConstants.ChampData.All);

                //var matches = await _summoner.RetrieveMatchHistory(SelectedRegion.Code);
                /* var summs = await _summoner.RetrievePlayerStatsSummaries(CreepScore.Season.Season2015);

                 var teams = await _summoner.RetrieveTeams();
                 var team = teams.First().Value.First();*/
                

                /*   var league = await _summoner.RetrieveLeague();

                   var leagueEntry = await _summoner.RetrieveLeagueEntry();
                
                   var l = league.First();
                   var y = league.First().Value.First().entries;

                   var t = (from x in y
                           where x.wins == 6
                           select x).ToList();*/
                
                //var games = await _summoner.RetrieveRecentGames();


                //var rg = await _api.RetrieveSummonerSpellData(SelectedRegion.Code, (int)_summoner.id, StaticDataConstants.SpellData.All);
                /*   var ss = await _summoner.RetrievePlayerStatsSummaries(CreepScore.Season.Season2015);
                
                   List<int> champs = new List<int>();
                   champs.Add(o.id);
                   var lst = await _summoner.RetrieveMatchHistory(SelectedRegion.Code, champs);*/


                ChampData cd = new ChampData() { Static = static1, Stats = o };
                _champData.Add(cd);
            }

            _summonerData = new SummonerData();         
            foreach (var o in (from x in _rankedStats.champions
                               where x.id == 0
                               select x))
            {
                _summonerData.Ranked = o;
            }

            var a = await _summoner.RetrievePlayerStatsSummaries(CreepScore.Season.Season2015);
            foreach (var o in (from x in a.playerStatSummaries
                               where x.playerStatSummaryTypeString == "RankedSolo5x5"
                               select x))
            {
                _summonerData.Normal = o;
            }
           // ss.playerStatSummaries.First().
           

            SummonerName = "Summoner Name...";

            OnPropertyChanged("SummonerInfoVisibility");
            OnPropertyChanged("SummonerTitle");
            OnPropertyChanged("SummonerLevel");
            OnPropertyChanged("TabPagesEnabled");
            
            OnPropertyChanged("Champ1Name");
            OnPropertyChanged("Champ2Name");
            OnPropertyChanged("Champ3Name");
            OnPropertyChanged("Champ4Name");
            OnPropertyChanged("Champ5Name");

            OnPropertyChanged("Champ1Games");
            OnPropertyChanged("Champ2Games");
            OnPropertyChanged("Champ3Games");
            OnPropertyChanged("Champ4Games");
            OnPropertyChanged("Champ5Games");

            OnPropertyChanged("Champ1Wins");
            OnPropertyChanged("Champ2Wins");
            OnPropertyChanged("Champ3Wins");
            OnPropertyChanged("Champ4Wins");
            OnPropertyChanged("Champ5Wins");

            OnPropertyChanged("Champ1Looses");
            OnPropertyChanged("Champ2Looses");
            OnPropertyChanged("Champ3Looses");
            OnPropertyChanged("Champ4Looses");
            OnPropertyChanged("Champ5Looses");

            OnPropertyChanged("Kills");
            OnPropertyChanged("Deaths");
            OnPropertyChanged("DoubleKills");
            OnPropertyChanged("TripleKills");
            OnPropertyChanged("QuadraKills");
            OnPropertyChanged("PentaKills");

            OnPropertyChanged("WinsRanked");
            OnPropertyChanged("LossesRanked");
            OnPropertyChanged("AssistsRanked");

            OnPropertyChanged("WinsNormal");
            OnPropertyChanged("LossesNormal");
            OnPropertyChanged("AssistsNormal");

            OnPropertyChanged("AverageKills");
            OnPropertyChanged("AverageDeaths");
            OnPropertyChanged("AverageGold");
            OnPropertyChanged("AverageFarm");
            OnPropertyChanged("TotalGames");

        }
    }
}
