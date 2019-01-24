using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperGra.Model
{
    public class Attribute
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public Attribute()
        {
        }

        public Attribute(string Name, int Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
    }
}
