using System.Collections.Generic;

namespace SuperGra.Model
{
    public class Character
    {
        public string       Id          { get; set; }
        public int          IdCharacter { get; set; }
        public string       Name        { get; set; }
        public string       Description { get; set; }
        public string       Class       { get; set; }
        public List<Stat>   Stats       { get; set; }
        public List<string> Equipment   { get; set; }

		public void Update(Character ch)
		{
			Name        = ch.Name;
			Description = ch.Description;
			Class       = ch.Class;
			Stats       = ch.Stats;
			Equipment   = ch.Equipment;
		}

		public override bool Equals(object obj)
		{
			return Id.Equals(((Character)obj).Id);
		}
	}
}