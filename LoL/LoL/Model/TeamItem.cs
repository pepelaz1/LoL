using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LoL.Model
{
    public class TeamItem : INotifyPropertyChanged
    {
        private String _name;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (value == _name)
                    return;

                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public ImageSource ChampImage { get; set; }
        public ImageSource SpellImageTop { get; set; }
        public ImageSource SpellImageBottom { get; set; }

        public String ChampName { get; set; }
        public String ChampGamesNum { get; set; }
        public ImageSource RankImage { get; set; }
        public String RankName { get; set; }
        public String RankScore { get; set; }
        public String Wins { get; set; }
        public String RankedWins { get; set; }
        public String RankedLosses { get; set; }
        public String Kills { get; set; }
        public String Deaths { get; set; }
        public String Assists { get; set; }
        public String Runes { get; set; }
        public String Offense { get; set; }
        public String Defense { get; set; }
        public String Utility { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
