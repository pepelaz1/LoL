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

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
