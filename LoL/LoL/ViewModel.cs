using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoL
{
    public class ViewModel : INotifyPropertyChanged
    {
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

    }
}
