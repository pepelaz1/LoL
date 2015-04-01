using LolSvlt.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace LolSvlt
{
    public class ViewModel :INotifyPropertyChanged
    {
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
                    Region r = new Region() { Name = "North America", Code = "NA"};
                    _selectedRegion = r;
                    _regions.Add(r);
                    _regions.Add(new Region() { Name = "Europe West", Code = "EUW" });
                    _regions.Add(new Region() { Name = "Europe Nordic & East", Code = "EUNE" });
                    _regions.Add(new Region() { Name = "Brazil", Code = "BR" });
                    _regions.Add(new Region() { Name = "Turkey", Code = "TR" });
                    _regions.Add(new Region() { Name = "Russia", Code = "RU" });
                    _regions.Add(new Region() { Name = "Latin Ameria North", Code = "LAN" });
                    _regions.Add(new Region() { Name = "Latin Ameria South", Code = "LAS" });
                    _regions.Add(new Region() { Name = "Oceania", Code = "OCE" });
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

        /*public Visibility SummonerInfoVisibility
        {
            get
            {
                return (_summonerData.Summoner == null) ? Visibility.Hidden : Visibility.Visible;
            }
        }*/

        public bool TabPagesEnabled
        {
            get
            {

                //return _summoner != null;
                return true;
            }
        }
    }
}
