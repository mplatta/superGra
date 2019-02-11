using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperGra.Dialogs
{
    public interface IDialogService
    {
        DialogResults<T> OpenDialog<T>(DialogViewModelBase<T> viewModel);
    }
}
