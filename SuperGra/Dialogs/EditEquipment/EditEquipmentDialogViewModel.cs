using SuperGra.Utilities;
using System.Diagnostics;
using System.Windows.Input;

namespace SuperGra.Dialogs.EditEquipment
{
    public class EditEquipmentDialogViewModel : DialogViewModelBase<DialogDecisions>
    {
        public ICommand ChangeCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public EditEquipmentDialogViewModel(string title, string equipment) : base(title, equipment)
        {
            ChangeCommand = new RelayCommand<IDialogWindow>(Change);
            CancelCommand = new RelayCommand<IDialogWindow>(Cancel);
        }        

        private void Change(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogDecisions.Yes);
        }

        private void Cancel(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogDecisions.Undefined);
        }
    }
}
