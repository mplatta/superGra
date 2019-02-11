using SuperGra.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperGra.Dialogs
{
    public enum DialogDecisions
    {
        Undefined,
        Yes,
        No
    }

    public class DialogResults<T>
    {
        public Stat MyStat { get; set; }
        public string Equipment { get; set; }
        public T decisions;
    }
}
