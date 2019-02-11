using SuperGra.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SuperGra.Dialogs.SendMessage
{
    public class SendMessageDialogViewModel : DialogViewModelBase<DialogDecisions>
    {
        public ICommand YesCommand { get; private set; }
        public ICommand NoCommand { get; private set; }

        public SendMessageDialogViewModel(string title, string message) : base(title, message)
        {
            YesCommand = new RelayCommand<IDialogWindow>(Yes);
            NoCommand = new RelayCommand<IDialogWindow>(No);
        }

        //public SendMessageDialogViewModel(string title, Stat obj) : base(title, obj)
        //{
        //    YesCommand = new RelayCommand<IDialogWindow>(Yes);
        //    NoCommand = new RelayCommand<IDialogWindow>(No);
        //}

        private void Yes(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogDecisions.Yes);
        }

        private void No(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogDecisions.No);
        }
    }
}
