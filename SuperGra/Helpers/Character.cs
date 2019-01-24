using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperGra.Helpers
{
	class Character
	{
		public string Id             { get; set; }
		public string NickName       { get; set; }
		public string Description    { get; set; }
		public string CharacterClass { get; set; }

		public Dictionary<string, string> Statistics { get; set; }
		public Dictionary<string, string> Equipment  { get; set; }

		public Character(string strJSON)
		{
			dynamic json = JsonConvert.DeserializeObject(strJSON);
			Id             = json.Id;
			NickName       = json.Name;
			CharacterClass = json.Class;

			Statistics = json.Stats.ToObject<Dictionary<string, object>>();
			Equipment  = json.Equipment.ToObject<Dictionary<string, object>>();
		}

		public override string ToString()
		{
			string tmp = Id + "--" + NickName + "--" + Description + "--" + CharacterClass + "\n";
			string tmp2 = "{" + string.Join(",", Statistics.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
			string tmp3 = "{" + string.Join(",", Equipment.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
			tmp += tmp2 + "\n" + tmp3;

			return tmp;
		}
	}
}
