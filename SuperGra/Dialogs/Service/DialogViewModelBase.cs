using SuperGra.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperGra.Dialogs
{
    public abstract class DialogViewModelBase<T>
    {
        public string Title { get; set; }
        public string Equipment { get; set; }
        public T DialogResult { get; set; }
        public Stat NewStat { get; set; }

        public DialogViewModelBase() : this(string.Empty, string.Empty) { }
        public DialogViewModelBase(string title) : this(title, string.Empty) { }
        public DialogViewModelBase(string title, string equipment)
        {
            Title = title;
            Equipment = equipment;
        }

        public DialogViewModelBase(string title, Stat stat)
        {
            Title = title;
            NewStat = stat;
        }

        public void CloseDialogWithResult(IDialogWindow dialog, T result)
        {
            DialogResult = result;

            if (dialog != null)
                dialog.DialogResult = true;
        }

        public void CloseDialogWithResult(IDialogWindow dialog, T result, Stat obj)
        {
            NewStat = obj;
            DialogResult = result;

            if (dialog != null)
                dialog.DialogResult = true;
        }

        public void CloseDialogWithResult(IDialogWindow dialog, T result, string item)
        {
            Equipment = item;
            DialogResult = result;

            if (dialog != null)
                dialog.DialogResult = true;
        }

        public void CloseDialogWithResult(IDialogWindow dialog, DialogResults<T> results)
        {
            NewStat = results.MyStat;
            DialogResult = results.decisions;

            if (dialog != null)
                dialog.DialogResult = true;
        }
    }
}
