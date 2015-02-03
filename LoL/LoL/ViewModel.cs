﻿using System;
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


        private String _summorer_name = "Summorer Name...";
        public String SummorerName
        {
            get
            {
                return _summorer_name;
            }
            set
            {
                _summorer_name = value;
                OnPropertyChanged("SummorerName");
            }
        }

    }
}
