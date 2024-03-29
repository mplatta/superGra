﻿using SuperGra.Model;
using SuperGra.Utilities;
using System.Windows.Input;

namespace SuperGra.Dialogs.EditStat
{
    public class EditStatDialogViewModel : DialogViewModelBase<DialogDecisions>
    {
        public ICommand ChangeCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public EditStatDialogViewModel(string title, Stat obj) : base(title, obj)
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
