using System.ComponentModel;
using SuperGra.Model;
using System.Collections.ObjectModel;
using SuperGra.Utilities;
using System.Windows;
using System.Diagnostics;
using SuperGra.Dialogs;
using System.Windows.Input;
using SuperGra.Dialogs.SendMessage;

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

        private IDialogService _dialogService;

        public ICommand SendMessageCommand { get; private set; }
        public ICommand EditStatCommand { get; private set; }

        public MainViewModel()
        {
            AllItems = new ObservableCollection<MyItem>();

            _dialogService = new DialogService();

            SendMessageCommand = new RelayCommand(SendMessage);
            EditStatCommand = new RelayCommand(EditStat);
        }

        private void SendMessage(object obj)
        {
            var dialog = new SendMessageDialogViewModel("Send Message", "Do you want send message to players?");
            var result = _dialogService.OpenDialog(dialog);
            Debug.WriteLine(result);
        }

        private void EditStat(object obj)
        {
            var dialog = new SendMessageDialogViewModel("Edit Attribute", "Coś tu wpisać");
            var result = _dialogService.OpenDialog(dialog);
            var tmp = obj as Stat;

            if(result == DialogResults.Yes)
            {
                tmp.Name = "What";
                tmp.Value = 50;
            }
        }


        void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public bool TestCanUse(object message)
        {
            if ((string)message == "Im a console!")
                return false;

            return true;
        }
    }
}
