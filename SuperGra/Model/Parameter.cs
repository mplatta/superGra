using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperGra.Model
{
    public class Parameter
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public Parameter()
        {
        }

        public Parameter(string Name, int Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
    }
}
