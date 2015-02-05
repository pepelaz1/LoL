using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RiotSharp;
using RiotSharp.SummonerEndpoint;

namespace LoL
{
    public class ViewModel : INotifyPropertyChanged
    {
        // RiotApi C# Wrapper 
        private RiotApi _api;

        // Riot api key generated at https://developer.riotgames.com/ (user - pepelaz1) and it's limits
        private const String _apiKey = "1502847e-8f39-4fe7-a338-1c0895692085";
        private const int _limit_per_10s = 10;
        private const int _limit_per_10m = 500;

        // Current summoner
        private Summoner _summoner = null;


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
                return (_summoner == null) ? "" : _summoner.Name;
            }
        }

        /// Property returning received summoner level
        public String SummonerLevel
        {
            get
            {
                return (_summoner == null) ? "" : _summoner.Level.ToString();
            }
        }

            /// <summary>
        /// View model constructor
        /// </summary>
        public ViewModel()
        {
            // Create api instance based on api key
            _api = RiotApi.GetInstance(_apiKey, _limit_per_10s, _limit_per_10m);
        }

        public void QuerySummoner()
        {
            _summoner = _api.GetSummoner(Region.euw, SummonerName);
            OnPropertyChanged("SummonerInfoVisibility");
            OnPropertyChanged("SummonerTitle");
            OnPropertyChanged("SummonerLevel");
        }

    }
}
