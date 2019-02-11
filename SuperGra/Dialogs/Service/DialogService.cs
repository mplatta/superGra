using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperGra.Dialogs
{
    public class DialogService : IDialogService
    {
        public DialogResults<T> OpenDialog<T>(DialogViewModelBase<T> viewModel)
        {
            IDialogWindow window = new DialogWindow();
            window.DataContext = viewModel;
            window.ShowDialog();

            DialogResults<T> dialogResults = new DialogResults<T>
            {
                decisions = viewModel.DialogResult,
                Equipment = viewModel.Equipment,
                MyStat = viewModel.NewStat
            };

            return dialogResults;
        }
    }
}
