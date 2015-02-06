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

            /// <summary>
        /// View model constructor
        /// </summary>
        public ViewModel()
        {
            // Create api instance based on api key
            //_api = RiotApi.GetInstance(_apiKey, _limit_per_10s, _limit_per_10m);
            //_api = RiotApi.GetInstance(_apiKey);
//            _api = new CreepScore(_apiKey, _limit_per_10s, _limit_per_10m);
             _api = new CreepScore(_apiKey, _limit_per_10s, _limit_per_10m);

           // CreepScore creepScore = new CreepScore("YOUR-API-KEY-GOES-HERE", 10, 500);
           // Summoner golf1052 = creepScore.RetrieveSummoner(CreepScore.Region.NA, "golf1052");
        }

        public async Task QuerySummoner()
        {
           // _summoner = _api.GetSummoner(Region.na, SummonerName);
            _summoner = null;
            _summoner = await _api.RetrieveSummoner(SelectedRegion.Code, SummonerName);
            if (_summoner == null)
                throw new Exception("Summoner not found");


         //   List<Champion> champs = await _api.RetrieveChampions(SelectedRegion.Code);


       //     var a =  await _api.RetrieveChampion(SelectedRegion.Code,(int) champs[0].id);
           
           // ChampionListStatic lst = await _api.RetrieveChampionsData(SelectedRegion.Code, StaticDataConstants.ChampData.All);
            //var a = await _api.RetrieveSummonerSpellData(SelectedRegion.Code, (int)_summoner.id, StaticDataConstants.SpellData.All);
            var a = await _summoner.RetrieveRankedStats(CreepScore.Season.Season2015);
          
            //_api.RetrieveChampionsData()
            SummonerName = "Summoner Name...";
            OnPropertyChanged("SummonerInfoVisibility");
            OnPropertyChanged("SummonerTitle");
            OnPropertyChanged("SummonerLevel");
        }

    }
}
