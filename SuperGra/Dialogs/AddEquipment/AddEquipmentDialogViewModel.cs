using SuperGra.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SuperGra.Dialogs.AddEquipment
{
    public class AddEquipmentDialogViewModel : DialogViewModelBase<object>
    {
        public ICommand AddCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public string NewItem { get; set; }

        public AddEquipmentDialogViewModel(string title) : base(title)
        {
            AddCommand = new RelayCommand<IDialogWindow>(Add);
            CancelCommand = new RelayCommand<IDialogWindow>(Cancel);
        }

        private void Add(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogDecisions.Yes, NewItem);
        }

        private void Cancel(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogDecisions.Undefined);
        }
    }
}
