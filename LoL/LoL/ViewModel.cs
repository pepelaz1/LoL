﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CreepScoreAPI;
using CreepScoreAPI.Constants;
using HtmlAgilityPack;
using LoL.Model;
using Newtonsoft.Json.Linq;


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

        // Error string
        private String _last_error;
        public String LastError
        {
            get {
                switch (_last_error)
                {
                    case "503":
                        return "Service unavailable";
                    case "404":
                        return "Summoner not found";
                    default:
                        return _last_error;
                }
            }
        }

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
                
                //return _summoner != null;
                return true;
            }
        }

        
        ///
        /// Champions data
        ///
        // Current summoner ranked stats
       // private RankedStats _rankedStats = null;

        // Current summoner's top 5champion data
        private List<ChampData> _champData = new List<ChampData>();
        public List<ChampData> ChampData
        {
            get { return _champData; }
        }

        private List<ChampData> _rchampData = new List<ChampData>();
        public List<ChampData> RChampData
        {
            get { return _rchampData; }
        }

        // Top 5 champs
        #region Top 5 champs
        public String Champ1Name 
        {
            get { return _champData.Count() > 0 ? _champData[0].Name : ""; }
        }

        public String Champ2Name
        {
            get { return _champData.Count() > 1 ? _champData[1].Name : ""; }
        }

        public String Champ3Name
        {
            get { return _champData.Count() > 2 ? _champData[2].Name : ""; }
        }

        public String Champ4Name
        {
            get { return _champData.Count() > 3 ? _champData[3].Name : ""; }
        }

        public String Champ5Name
        {
            get { return _champData.Count() > 4 ? _champData[4].Name : ""; }
        }

        public ImageSource Champ1Image
        {
            get
            {
                return _champData.Count() > 0 ? _champData[0].Picture : null;
            }
        }

        public ImageSource Champ2Image
        {
            get {
                return _champData.Count() > 1 ? _champData[1].Picture : null; 
            }
        }

        public ImageSource Champ3Image
        {
            get
            {
                return _champData.Count() > 2 ? _champData[2].Picture : null;
            }
        }

        public ImageSource Champ4Image
        {
            get
            {
                return _champData.Count() > 3 ? _champData[3].Picture : null;
            }
        }

        public ImageSource Champ5Image
        {
            get
            {
                return _champData.Count() > 4 ? _champData[4].Picture : null;
            }
        }   

        public String Champ1Kills
        {
            get { return _champData.Count() > 0 ? _champData[0].Kills : ""; }
        }

        public String Champ2Kills
        {
            get { return _champData.Count() > 1 ? _champData[1].Kills : ""; }
        }

        public String Champ3Kills
        {
            get { return _champData.Count() > 2 ? _champData[2].Kills : ""; }
        }

        public String Champ4Kills
        {
            get { return _champData.Count() > 3 ? _champData[3].Kills : ""; }
        }

        public String Champ5Kills
        {
            get { return _champData.Count() > 4 ? _champData[4].Kills : ""; }
        }

        public String Champ1Deaths
        {
            get { return _champData.Count() > 0 ? _champData[0].Deaths : ""; }
        }

        public String Champ2Deaths
        {
            get { return _champData.Count() > 1 ? _champData[1].Deaths : ""; }
        }

        public String Champ3Deaths
        {
            get { return _champData.Count() > 2 ? _champData[2].Deaths : ""; }
        }

        public String Champ4Deaths
        {
            get { return _champData.Count() > 3 ? _champData[3].Deaths : ""; }
        }

        public String Champ5Deaths
        {
            get { return _champData.Count() > 4 ? _champData[4].Deaths : ""; }
        }

        public String Champ1Assists
        {
            get { return _champData.Count() > 0 ? _champData[0].Assists : ""; }
        }

        public String Champ2Assists
        {
            get { return _champData.Count() > 1 ? _champData[1].Assists : ""; }
        }

        public String Champ3Assists
        {
            get { return _champData.Count() > 2 ? _champData[2].Assists : ""; }
        }

        public String Champ4Assists
        {
            get { return _champData.Count() > 3 ? _champData[3].Assists : ""; }
        }

        public String Champ5Assists
        {
            get { return _champData.Count() > 4 ? _champData[4].Assists : ""; }
        }

        public String Champ1WL
        {
            get { return _champData.Count() > 0 ? _champData[0].Wins + "/" + _champData[0].Looses : ""; }
        }

        public String Champ2WL
        {
            get { return _champData.Count() > 1 ? _champData[1].Wins + "/" + _champData[1].Looses : ""; }
        }

        public String Champ3WL
        {
            get { return _champData.Count() > 2 ? _champData[2].Wins + "/" + _champData[2].Looses : ""; }
        }

        public String Champ4WL
        {
            get { return _champData.Count() > 3 ? _champData[3].Wins + "/" + _champData[3].Looses : ""; }
        }

        public String Champ5WL
        {
            get { return _champData.Count() > 4 ? _champData[4].Wins + "/" + _champData[4].Looses : ""; }
        }
        #endregion

        #region Recent champs
        // Recent champs 
        public String RChamp1Name
        {
            get { return _rchampData.Count() > 0 ? _rchampData[0].Name : ""; }
        }

        public String RChamp2Name
        {
            get { return _rchampData.Count() > 1 ? _rchampData[1].Name : ""; }
        }

        public String RChamp3Name
        {
            get { return _rchampData.Count() > 2 ? _rchampData[2].Name : ""; }
        }

        public String RChamp4Name
        {
            get { return _rchampData.Count() > 3 ? _rchampData[3].Name : ""; }
        }

        public ImageSource RChamp1Image
        {
            get { return _rchampData.Count() > 0 ? _rchampData[0].Picture : null;   }
        }

        public ImageSource RChamp2Image
        {
            get { return _rchampData.Count() > 1 ? _rchampData[1].Picture : null;
            }
        }

        public ImageSource RChamp3Image
        {
            get {  return _rchampData.Count() > 2 ? _rchampData[2].Picture : null; }
        }

        public ImageSource RChamp4Image
        {
            get { return _rchampData.Count() > 3 ? _rchampData[3].Picture : null;  }
        }

        public String RChamp1Kills
        {
            get { return _rchampData.Count() > 0 ? _rchampData[0].Kills : ""; }
        }

        public String RChamp2Kills
        {
            get { return _rchampData.Count() > 1 ? _rchampData[1].Kills : ""; }
        }

        public String RChamp3Kills
        {
            get { return _rchampData.Count() > 2 ? _rchampData[2].Kills : ""; }
        }

        public String RChamp4Kills
        {
            get { return _rchampData.Count() > 3 ? _rchampData[3].Kills : ""; }
        }

      
        public String RChamp1Deaths
        {
            get { return _rchampData.Count() > 0 ? _rchampData[0].Deaths : ""; }
        }

        public String RChamp2Deaths
        {
            get { return _rchampData.Count() > 1 ? _rchampData[1].Deaths : ""; }
        }

        public String RChamp3Deaths
        {
            get { return _rchampData.Count() > 2 ? _rchampData[2].Deaths : ""; }
        }

        public String RChamp4Deaths
        {
            get { return _rchampData.Count() > 3 ? _rchampData[3].Deaths : ""; }
        }


        public String RChamp1Assists
        {
            get { return _rchampData.Count() > 0 ? _rchampData[0].Assists : ""; }
        }

        public String RChamp2Assists
        {
            get { return _rchampData.Count() > 1 ? _rchampData[1].Assists : ""; }
        }

        public String RChamp3Assists
        {
            get { return _rchampData.Count() > 2 ? _rchampData[2].Assists : ""; }
        }

        public String RChamp4Assists
        {
            get { return _rchampData.Count() > 3 ? _rchampData[3].Assists : ""; }
        }

        public String RChamp1WL
        {
            get { return _rchampData.Count() > 0 ? _rchampData[0].Wins + "/" + _rchampData[0].Looses : ""; }
        }

        public String RChamp2WL
        {
            get { return _rchampData.Count() > 1 ? _rchampData[1].Wins + "/" + _rchampData[1].Looses : ""; }
        }

        public String RChamp3WL
        {
            get { return _rchampData.Count() > 2 ? _rchampData[2].Wins + "/" + _rchampData[2].Looses : ""; }
        }

        public String RChamp4WL
        {
            get { return _rchampData.Count() > 3 ? _rchampData[3].Wins + "/" + _rchampData[3].Looses : ""; }
        }
        #endregion

        ///
        /// Summoner data
        /// 
        private SummonerData _summonerData = new SummonerData();

        // Ranked 5v5 stat
        public String KillsRanked
        {
            get { return _summonerData.Ranked.stats.totalChampionKills.ToString(); }
        }

        public String DeathsRanked
        {
            get { return _summonerData.Ranked.stats.totalDeathsPerSession.ToString(); }
        }

        public String AssistsRanked
        {
            get { return _summonerData.Ranked.stats.totalAssists.ToString(); }
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


        public String WLRanked
        {
            get { return WinsRanked + "/" + LossesRanked; }
        }
    

        // Normal 5v5 stat
        public String KillsNormal
        {
            get { return _summonerData.KillsNormal; }
        }
     
        public String AssistsNormal
        {
            get { return _summonerData.AssistsNormal; }
        }

        public String WinsNormal
        {
            get { return _summonerData.WinsNormal; }
        }

       /* public String LossesNormal
        {
            get { return _summonerData.Normal.losses.ToString(); }
        }

  
        public String WLNormal
        {
            get { return WinsNormal + "/" + LossesNormal; }
        }*/
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

        public String AverageAssist
        {
            get { return (_summonerData.Ranked.stats.totalAssists / _summonerData.Ranked.stats.totalSessionsPlayed).ToString(); }
        }

        public string AverageKDA
        {
            get { return AverageKills + "/" + AverageDeaths + "/" + AverageAssist; }
        }

        public String AverageGold
        {
            get { return ((int)(_summonerData.Ranked.stats.totalGoldEarned / _summonerData.Ranked.stats.totalSessionsPlayed)).ToString("N0"); }
        }

        public String AverageFarm
        {
            get { return (_summonerData.Ranked.stats.totalMinionKills / _summonerData.Ranked.stats.totalSessionsPlayed).ToString(); }
        }
        public String TotalGames
        {
            get { return _summonerData.Ranked.stats.totalSessionsPlayed.ToString(); }
        }

        public String TotalHoursPlayed
        {
            get { return _summonerData.TotalHoursPlayed; }
        }

        public String TotalDaysPlayed
        {
            get { return _summonerData.TotalDaysPlayed; }
        }

        public String Warding
        {
            get { return _summonerData.Warding; }
        }

        public String WardScore
        {
            get { return _summonerData.WardScore; }
        }

        public String WardMage
        {
            get { return _summonerData.WardMage; }
        }
        public String WardAssasin
        {
            get { return _summonerData.WardAssasin; }
        }
        public String WardMarksman
        {
            get { return _summonerData.WardMarksman; }
        }
        public String WardFighter
        {
            get { return _summonerData.WardFighter; }
        }
        public String WardTank
        {
            get { return _summonerData.WardTank; }
        }

        public String WardSupport
        {
            get { return _summonerData.WardSupport; }
        }


        // Personal Ratings
        public ImageSource Team3v3Image
        {
            get {return _summonerData.Team3v3Image ;}
        }
        public String Team3v3Rating
        {
            get { return _summonerData.Team3v3Rating; }
        }
        public String Team3v3LeaguePoints
        {
            get { return _summonerData.Team3v3LeaguePoints; }
        }
        public String Team3v3Wins
        {
            get { return _summonerData.Team3v3Wins; }
        }
        public String Team3v3Looses
        {
            get { return _summonerData.Team3v3Looses; }
        }
        public ImageSource Solo5v5Image
        {
            get { return _summonerData.Solo5v5Image; }
        }
        public String Solo5v5Rating
        {
            get { return _summonerData.Solo5v5Rating; }
        }
        public String Solo5v5LeaguePoints
        {
            get { return _summonerData.Solo5v5LeaguePoints; }
        }
        public String Solo5v5Wins
        {
            get { return _summonerData.Solo5v5Wins; }
        }
        public String Solo5v5Looses
        {
            get { return _summonerData.Solo5v5Looses; }
        }
        public ImageSource Team5v5Image 
        {
            get { return _summonerData.Team5v5Image; }
        }
        public String Team5v5Rating
        {
            get { return _summonerData.Team5v5Rating; }
        }
        public String Team5v5LeaguePoints
        {
            get { return _summonerData.Team5v5LeaguePoints; }
        }
        public String Team5v5Wins
        {
            get { return _summonerData.Team5v5Wins; }
        }
        public String Team5v5Looses
        {
            get { return _summonerData.Team5v5Looses; }
        }
        /// <summary>
        /// View model constructor
        /// </summary>
        public ViewModel()
        {
            // _api = new CreepScore(_apiKey, _limit_per_10s, _limit_per_10m);
        }

        private void QueryTotalTime()
        {
            try
            {
                // Query total time information from the wastedonlol website
                String url = "https://wastedonlol.com/" + SelectedRegion.Code.ToString() + "-" + _summoner_name + "/";
                var web = new HtmlWeb();
                var doc = web.Load(url);
                foreach (var node in doc.DocumentNode.SelectNodes("//span[@class='result_pseudo']"))
                {
                    _summonerData.TotalHoursPlayed = (node.ChildNodes[2].InnerText).Replace('.',',');
                    _summonerData.TotalDaysPlayed = (node.ChildNodes[4].InnerText).Replace('.', ',');
                }               
            }
            catch(Exception)
            {
                _summonerData.TotalHoursPlayed = "N/A";
                _summonerData.TotalDaysPlayed = "N/A";
            }
        }

        private void QueryWardScore()
        {
            try
            {
                String url = "http://wardscore.loltools.net/wards_core.php?name=" + _summoner_name + "&region=" + SelectedRegion.Code.ToString().ToLower();
                var json = new WebClient().DownloadString(url);
                JObject o = JObject.Parse(json);

                _summonerData.Warding = (String)o["yourwarding"];
                _summonerData.WardScore = (String)o["score"];
                _summonerData.WardMage = ((String)o["wpg"]["wpg_mage"]).Replace(" wpg", "");
                _summonerData.WardAssasin = ((String)o["wpg"]["wpg_assasin"]).Replace(" wpg", "");
                _summonerData.WardMarksman = ((String)o["wpg"]["wpg_marksman"]).Replace(" wpg", "");
                _summonerData.WardFighter = ((String)o["wpg"]["wpg_fighter"]).Replace(" wpg", "");
                _summonerData.WardTank = ((String)o["wpg"]["wpg_tank"]).Replace(" wpg", "");
                _summonerData.WardSupport = ((String)o["wpg"]["wpg_support"]).Replace(" wpg", ""); 

            }
            catch (Exception ex)
            {
                _summonerData.WardScore = "N/A";
                _summonerData.WardMage = "N/A";
                _summonerData.WardAssasin = "N/A";
                _summonerData.WardMarksman = "N/A";
                _summonerData.WardFighter = "N/A";
                _summonerData.WardTank = "N/A";
                _summonerData.WardSupport = "N/A";
            }
        }

   
        public void QueryChampionImages()
        {
            try
            {
                var web = new HtmlWeb();
                var doc = web.Load("http://leagueoflegends.wikia.com/wiki/Category:Champion_squares");
                for (int i = 0; i < _champData.Count();i++ )
                {
                    String href = "/wiki/File:" + _champData[i].Static.name + "Square.png";

                    foreach (var img in (from x in doc.DocumentNode.SelectNodes("//a[@href][@class='image']")
                                         where x.Attributes["href"].Value == href 
                                         select x.ChildNodes[0]))
                    {

                        _champData[i].Picture = LoadImageFromURL(img.Attributes[6].Value);
                    }
                }
             
            }
            catch (Exception)
            {
            }
        }

        public void QueryLolking()
        {
            try
            {
                Regex regex = new Regex(@"//([-\w\.]+)+(:\d+)?(/([-\w/_\.]*(\?\S+)?)?)?");
                Regex rgnums = new Regex(@"[0-9]+");

                String url = "http://www.lolking.net/search?name=" + _summoner_name + "&region=" + SelectedRegion.Code.ToString() + "";
       
                // Query total time information from the lolking website
                //  String url = "http://www.lolking.net/summoner/" + SelectedRegion.Code.ToString().ToLower() + "/" + _summoner.id + "";
              
                  
                var web = new HtmlWeb();
                var doc = web.Load(url);              

                // Ranked stat
                foreach (var node in doc.DocumentNode.SelectNodes("//div[@class='lifetime_stats_header']"))
                {
                    if (node.InnerText == "Normal 5v5")
                    {
                        var vals = node.NextSibling.NextSibling.SelectNodes("tbody/tr/td[@class='lifetime_stats_val']");
                        _summonerData.KillsNormal = vals[1].InnerText;
                        _summonerData.WinsNormal = vals[0].InnerText;
                        _summonerData.AssistsNormal = vals[2].InnerText;
                    }
                }

                String ss = "";
                foreach (var node in (from  x in doc.DocumentNode.SelectNodes("//script")
                                      where x.InnerText.Contains("window.LOLKING")
                                      select x))
                {
                    ss = node.InnerText;
                }

                //String ss = doc.DocumentNode.SelectNode("//script").InnerText;
                ss = ss.Substring(ss.IndexOf("champions = ") + 12, ss.IndexOf("}}") - ss.IndexOf("champions = ")-10);
                JObject o = JObject.Parse(ss);
                IDictionary<string, JToken> objects = (JObject)o;
                Dictionary<string, JObject> dictionary = objects.ToDictionary(pair => pair.Key, pair => (JObject)pair.Value);

            
                // Top 5 champs
                 _champData.Clear();
                foreach( var node in (from x in doc.DocumentNode.SelectNodes("//table[@class='clientsort season_5_ranked_stats']//tbody//tr")
                                      orderby int.Parse(x.ChildNodes[3].InnerText) descending
                                      select x).Take(5))
                {

                     ChampData cd = new ChampData() 
                     {
                         Kills = node.ChildNodes[11].InnerText.Split("/".ToCharArray())[0],
                         Deaths = node.ChildNodes[13].InnerText.Split("/".ToCharArray())[0],
                         Assists = node.ChildNodes[15].InnerText.Split("/".ToCharArray())[0],
                         Name = node.ChildNodes[1].InnerText.TrimStart("\n ".ToCharArray()).TrimEnd("\n ".ToCharArray()),
                         Wins = node.ChildNodes[3].InnerText,
                         Looses = node.ChildNodes[5].InnerText };
                    Match match = regex.Match(node.ChildNodes[1].ChildNodes[1].ChildNodes[1].Attributes["style"].Value.Replace("_32", ""));
                    if (match.Success)
                    {
                        String val = "http:" + match.Value;
                        cd.Picture = LoadImageFromURL(val);
                    }
                    _champData.Add(cd);
                }
                
                // Recent champs            
                foreach (var node in doc.DocumentNode.SelectNodes("//div[@class='recent_statistics_champion_icon']"))
                {
                    String s = node.Attributes["style"].Value;
                    Match match = regex.Match(s);
                    if (match.Success)
                    {
                        Match m2 = rgnums.Match(match.Value);
                        String id = m2.Value;
                        String name = "";
                        foreach (var l in (from x in dictionary.Values
                                           where x["champion_id"].ToString() == id
                                           select x))
                        {
                           name = l["name"].ToString();
                           break;
                        }

                        String val = "http:" + match.Value;
                        ChampData cd = new ChampData()
                        {
                            Name = name,
                            Kills = node.SelectNodes("div")[2].SelectNodes("div")[0].SelectNodes("div")[0].SelectNodes("div")[0].InnerText,
		                    Deaths = node.SelectNodes("div")[2].SelectNodes("div")[1].SelectNodes("div")[0].SelectNodes("div")[0].InnerText,
		                    Assists = node.SelectNodes("div")[2].SelectNodes("div")[2].SelectNodes("div")[0].SelectNodes("div")[0].InnerText,
                            Wins = node.SelectNodes("div")[1].SelectNodes("div")[0].SelectNodes("span")[0].ChildNodes[0].InnerText,
                            Looses = node.SelectNodes("div")[1].SelectNodes("div")[0].SelectNodes("span")[1].ChildNodes[0].InnerText

                        };
                        cd.Picture = LoadImageFromURL(val);
                        _rchampData.Add(cd);
                    }
                }


                // Personal ratings
                foreach (var node in doc.DocumentNode.SelectNodes("//div[@class='personal_ratings_heading']"))
                {
                    if (node.InnerText == "Team 3v3")
                    {
                        Match match = regex.Match(node.NextSibling.NextSibling.Attributes["style"].Value);
                        if (match.Success)
                        {
                            _summonerData.Team3v3Image = LoadImageFromURL("http:" + match.Value);
                            var node2 =  node.NextSibling.NextSibling.NextSibling.NextSibling.SelectNodes("div[@class='personal_ratings_rating']")[0];
                            _summonerData.Team3v3Rating = node2.InnerText;
                            _summonerData.Team3v3LeaguePoints = node2.NextSibling.NextSibling.InnerText;

                            var node3 = node.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling;
                            _summonerData.Team3v3Wins = node3.SelectNodes("span[@class='personal_ratings_wins']")[0].InnerText;
                            _summonerData.Team3v3Looses = node3.NextSibling.NextSibling.SelectNodes("span[@class='personal_ratings_losses']")[0].InnerText;
                        }
                    }
                    else if (node.InnerText == "Solo 5v5")
                    {
                        Match match = regex.Match(node.NextSibling.NextSibling.Attributes["style"].Value);
                        if (match.Success)
                        {
                            _summonerData.Solo5v5Image = LoadImageFromURL("http:" + match.Value);
                            var node2 = node.NextSibling.NextSibling.NextSibling.NextSibling.SelectNodes("div[@class='personal_ratings_rating']")[0];
                            _summonerData.Solo5v5Rating = node2.InnerText;
                            _summonerData.Solo5v5LeaguePoints = node2.NextSibling.NextSibling.InnerText;

                            var node3 = node.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling;
                            _summonerData.Solo5v5Wins = node3.SelectNodes("span[@class='personal_ratings_wins']")[0].InnerText;
                            _summonerData.Solo5v5Looses = node3.NextSibling.NextSibling.SelectNodes("span[@class='personal_ratings_losses']")[0].InnerText;
                        }
                    }
                    else if (node.InnerText == "Team 5v5")
                    {
                        Match match = regex.Match(node.NextSibling.NextSibling.Attributes["style"].Value);
                        if (match.Success)
                        {
                            _summonerData.Team5v5Image = LoadImageFromURL("http:" + match.Value);
                            var node2 = node.NextSibling.NextSibling.NextSibling.NextSibling.SelectNodes("div[@class='personal_ratings_rating']")[0];
                            _summonerData.Team5v5Rating = node2.InnerText;
                            _summonerData.Team5v5LeaguePoints = node2.NextSibling.NextSibling.InnerText;

                            var node3 = node.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling;
                            _summonerData.Team5v5Wins = node3.SelectNodes("span[@class='personal_ratings_wins']")[0].InnerText;
                            _summonerData.Team5v5Looses = node3.NextSibling.NextSibling.SelectNodes("span[@class='personal_ratings_losses']")[0].InnerText;
                        }
                    } 

                }
            }
            catch (Exception ex)
            {
                int t = 4;
                t = 4;
            }
        }

        public BitmapImage LoadImageFromURL(String url)
        {
            var image = new BitmapImage();
            int BytesToRead = 100;

            WebRequest request = WebRequest.Create(new Uri(url, UriKind.Absolute));
            request.Timeout = -1;
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            BinaryReader reader = new BinaryReader(responseStream);
            MemoryStream memoryStream = new MemoryStream();

            byte[] bytebuffer = new byte[BytesToRead];
            int bytesRead = reader.Read(bytebuffer, 0, BytesToRead);

            while (bytesRead > 0)
            {
                memoryStream.Write(bytebuffer, 0, bytesRead);
                bytesRead = reader.Read(bytebuffer, 0, BytesToRead);
            }

            image.BeginInit();
            memoryStream.Seek(0, SeekOrigin.Begin);

            image.StreamSource = memoryStream;
            image.EndInit();
            return image;
        }

        public async Task QueryData()
        {                  
          //  _summoner = null;
          //  _summoner = await _api.RetrieveSummoner(SelectedRegion.Code, SummonerName);
          //  if (_summoner == null)
          //  {
           //     _last_error = _api.ErrorString;
           //     throw new Exception("");
           // }

           // _rankedStats = await _summoner.RetrieveRankedStats(CreepScore.Season.Season2015);
          //  _champData.Clear();
            
     
           /* foreach (var o in (from x in _rankedStats.champions
                               where x.id != 0
                               orderby x.stats.totalSessionsWon descending, x.stats.totalSessionsPlayed descending
                               select x).Take(5))
            {
                var static1 = await _api.RetrieveChampionData(SelectedRegion.Code, o.id, StaticDataConstants.ChampData.All);
                ChampData cd = new ChampData() { Static = static1, Stats = o };
                _champData.Add(cd);
            }*/

          /*  _summonerData = new SummonerData();         
            /*foreach (var o in (from x in _rankedStats.champions
                               where x.id == 0
                               select x))
            {
                _summonerData.Ranked = o;
            }*/

           /* var a = await _summoner.RetrievePlayerStatsSummaries(CreepScore.Season.Season2015);
            foreach (var o in (from x in a.playerStatSummaries
                               where x.playerStatSummaryTypeString == "Unranked"
                               select x))
            {
                _summonerData.Normal = o;
            }*/


            QueryLolking();
            QueryTotalTime();
            QueryWardScore();
          //  QueryChampionImages();
         
           

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

            OnPropertyChanged("Champ1Image");
            OnPropertyChanged("Champ2Image");
            OnPropertyChanged("Champ3Image");
            OnPropertyChanged("Champ4Image");
            OnPropertyChanged("Champ5Image");

            OnPropertyChanged("Champ1WL");
            OnPropertyChanged("Champ2WL");
            OnPropertyChanged("Champ3WL");
            OnPropertyChanged("Champ4WL");
            OnPropertyChanged("Champ5WL");

            OnPropertyChanged("Champ1Kills");
            OnPropertyChanged("Champ2Kills");
            OnPropertyChanged("Champ3Kills");
            OnPropertyChanged("Champ4Kills");
            OnPropertyChanged("Champ5Kills");

            OnPropertyChanged("Champ1Deaths");
            OnPropertyChanged("Champ2Deaths");
            OnPropertyChanged("Champ3Deaths");
            OnPropertyChanged("Champ4Deaths");
            OnPropertyChanged("Champ5Deaths");

            OnPropertyChanged("Champ1Assists");
            OnPropertyChanged("Champ2Assists");
            OnPropertyChanged("Champ3Assists");
            OnPropertyChanged("Champ4Assists");
            OnPropertyChanged("Champ5Assists");

            OnPropertyChanged("KillsRanked");
            OnPropertyChanged("DeathsRanked");
            OnPropertyChanged("AssistsRanked");
            OnPropertyChanged("DoubleKills");
            OnPropertyChanged("TripleKills");
            OnPropertyChanged("QuadraKills");
            OnPropertyChanged("PentaKills");
            OnPropertyChanged("WinsRanked");
            OnPropertyChanged("LossesRanked");
            OnPropertyChanged("WLRanked");


            OnPropertyChanged("KillsNormal");
            OnPropertyChanged("DeathsNormal");
            OnPropertyChanged("AssistsNormal");
            OnPropertyChanged("WinsNormal");
            OnPropertyChanged("LossesNormal");
            OnPropertyChanged("WLNormal");

            OnPropertyChanged("AverageKills");
            OnPropertyChanged("AverageDeaths");
            OnPropertyChanged("AverageGold");
            OnPropertyChanged("AverageFarm");
            OnPropertyChanged("AverageKDA");
            OnPropertyChanged("TotalGames");
            OnPropertyChanged("TotalHoursPlayed");
            OnPropertyChanged("TotalDaysPlayed");

            OnPropertyChanged("Warding");
            OnPropertyChanged("WardScore");
            OnPropertyChanged("WardMage");
            OnPropertyChanged("WardAssasin");
            OnPropertyChanged("WardMarksman");
            OnPropertyChanged("WardFighter");
            OnPropertyChanged("WardTank");
            OnPropertyChanged("WardSupport");

            OnPropertyChanged("RChamp1Name");
            OnPropertyChanged("RChamp2Name");
            OnPropertyChanged("RChamp3Name");
            OnPropertyChanged("RChamp4Name");
            OnPropertyChanged("RChamp5Name");

            OnPropertyChanged("RChamp1Image");
            OnPropertyChanged("RChamp2Image");
            OnPropertyChanged("RChamp3Image");
            OnPropertyChanged("RChamp4Image");
            OnPropertyChanged("RChamp5Image");

            OnPropertyChanged("RChamp1WL");
            OnPropertyChanged("RChamp2WL");
            OnPropertyChanged("RChamp3WL");
            OnPropertyChanged("RChamp4WL");
            OnPropertyChanged("RChamp5WL");

            OnPropertyChanged("RChamp1Kills");
            OnPropertyChanged("RChamp2Kills");
            OnPropertyChanged("RChamp3Kills");
            OnPropertyChanged("RChamp4Kills");
            OnPropertyChanged("RChamp5Kills");

            OnPropertyChanged("RChamp1Deaths");
            OnPropertyChanged("RChamp2Deaths");
            OnPropertyChanged("RChamp3Deaths");
            OnPropertyChanged("RChamp4Deaths");
            OnPropertyChanged("RChamp5Deaths");

            OnPropertyChanged("RChamp1Assists");
            OnPropertyChanged("RChamp2Assists");
            OnPropertyChanged("RChamp3Assists");
            OnPropertyChanged("RChamp4Assists");
            OnPropertyChanged("RChamp5Assists");

            OnPropertyChanged("Team3v3Image");
            OnPropertyChanged("Solo5v5Image");
            OnPropertyChanged("Team5v5Image");

            OnPropertyChanged("Team3v3Rating");
            OnPropertyChanged("Solo5v5Rating");
            OnPropertyChanged("Team5v5Rating");

            OnPropertyChanged("Team3v3LeaguePoints");
            OnPropertyChanged("Solo5v5LeaguePoints");
            OnPropertyChanged("Team5v5LeaguePoints");

            OnPropertyChanged("Team3v3Wins");
            OnPropertyChanged("Solo5v5Wins");
            OnPropertyChanged("Team5v5Wins");

            OnPropertyChanged("Team3v3Looses");
            OnPropertyChanged("Solo5v5Looses");
            OnPropertyChanged("Team5v5Looses");
       }
    }


}
