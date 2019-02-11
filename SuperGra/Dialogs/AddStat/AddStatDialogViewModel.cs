using SuperGra.Model;
using SuperGra.Utilities;
using System.Diagnostics;
using System.Windows.Input;

namespace SuperGra.Dialogs.AddStat
{
    public class AddStatDialogViewModel : DialogViewModelBase<object>
    {
        public ICommand AddCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public Stat MyStat { get; set; }

        public AddStatDialogViewModel(string title) : base(title)
        {
            MyStat = new Stat();
            AddCommand = new RelayCommand<IDialogWindow>(Add);
            CancelCommand = new RelayCommand<IDialogWindow>(Cancel);
        }

        private void Add(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogDecisions.Yes, MyStat);
        }

        private void Cancel(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogDecisions.Undefined);
        }
    }
}
