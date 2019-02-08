using SuperGra.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SuperGra.Dialogs.EditStat
{
    public class EditStatDialogViewModel : DialogViewModelBase<object>
    {
        public ICommand YesCommand { get; private set; }
        public ICommand NoCommand { get; private set; }

        public EditStatDialogViewModel(string title, string message) : base(title, message)
        {
            YesCommand = new RelayCommand<IDialogWindow>(Yes);
            NoCommand = new RelayCommand<IDialogWindow>(No);
        }

        private void Yes(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResults.Yes);
        }

        private void No(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResults.No);
        }
    }
}
