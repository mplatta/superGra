using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SuperGra.Model;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows.Media;

namespace SuperGra.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        ObservableCollection<MyItem> _AllItems;
        public ObservableCollection<MyItem> AllItems
        {
            get
            {
                return _AllItems;
            }
            set
            {
                if (_AllItems != value)
                {
                    _AllItems = value;
                    RaisePropertyChanged("AllItems");
                }
            }
        }

        public MainViewModel()
        {
            AllItems = new ObservableCollection<MyItem>();
        }


        void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
