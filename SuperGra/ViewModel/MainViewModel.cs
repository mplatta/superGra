using System.ComponentModel;
using SuperGra.Model;
using System.Collections.ObjectModel;
using SuperGra.Utilities;
using System.Windows;
using System.Diagnostics;
using SuperGra.Dialogs;
using System.Windows.Input;
using SuperGra.Dialogs.SendMessage;
using SuperGra.Dialogs.EditStat;
using SuperGra.Dialogs.EditEquipment;
using System;
using SuperGra.Dialogs.AddStat;
using System.Windows.Controls;
using SuperGra.Dialogs.AddEquipment;

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
        public ICommand AddStatCommand { get; private set; }
        public ICommand AddEquipmentCommand { get; private set; }
        public ICommand EditStatCommand { get; private set; }
        public ICommand EditEquipmentCommand { get; private set; }
        public ICommand DeleteStatCommand { get; private set; }

        public MainViewModel()
        {
            AllItems = new ObservableCollection<MyItem>();

            _dialogService = new DialogService();

            SendMessageCommand = new RelayCommand(SendMessage);
            EditStatCommand = new RelayCommand(EditStat);
            EditEquipmentCommand = new RelayCommand(EditEquipment);
            AddStatCommand = new RelayCommand(AddStat);
            AddEquipmentCommand = new RelayCommand(AddEquipment);
            DeleteStatCommand = new RelayCommand(DeleteStat);
        }

        private void AddEquipment(object obj)
        {
            var dialog = new AddEquipmentDialogViewModel("Add Equipment");
            var result = _dialogService.OpenDialog(dialog);
            ((MyItem)obj).CharacterCard.Equipment.Add(result.Equipment);
        }

        private void AddStat(object obj)
        {
            var dialog = new AddStatDialogViewModel("Add Attribute");
            var result = _dialogService.OpenDialog(dialog);
            ((MyItem)obj).CharacterCard.Stats.Add(result.MyStat);
        }

        private void DeleteStat(object obj)
        {
            //var dialog = MessageBox.Show("Are you sure?", "Title", MessageBoxButton.OKCancel);
            
            //if (dialog == MessageBoxResult.OK)
            //{
            //    Debug.WriteLine(obj);
            //}
        }

        private void SendMessage(object obj)
        {
            var dialog = new SendMessageDialogViewModel("Send Message", "Do you want send message to players?");
            var result = _dialogService.OpenDialog(dialog);
        }

        private void EditStat(object obj)
        {
            var tmp = obj as Stat;
            Stat editStatParameter = new Stat();
            editStatParameter.Name = tmp.Name;
            editStatParameter.Value = tmp.Value;
            var dialog = new EditStatDialogViewModel("Edit Attribute", editStatParameter);
            var result = _dialogService.OpenDialog(dialog);

            if (result.decisions == DialogDecisions.Yes)
            {
                tmp.Name = editStatParameter.Name;
                tmp.Value = editStatParameter.Value;
            }
        }

        private void EditEquipment(object equipment)
        {
            string tmp = equipment.ToString();
            var dialog = new EditEquipmentDialogViewModel("Edit Equipment", tmp);
            var result = _dialogService.OpenDialog(dialog);
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
