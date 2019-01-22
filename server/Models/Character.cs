
using System;
using System.Collections.Generic;

namespace server.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Class { get; set; }
        public List<Stat> Stats { get; set; }
        public List<string> Equipment { get; set; }      

    }
}