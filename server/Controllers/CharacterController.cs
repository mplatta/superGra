using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace server.Controllers
{
    public class CharacterController : ApiController
    {        

        [HttpGet]
        public IEnumerable<Character> GetAllCharacters()
        {
            Database db = Database.Instance;
            List<Character> characters = db.GetCharacters();

            return characters;
        }

        [HttpGet]
        public IHttpActionResult GetCharacter(int id)
        {
            Database db = Database.Instance;
            var character = db.GetCharacter(id);
           
            if (character == null)
            {
                return NotFound();
            }

            return Ok(character);
        }        

        [Route("api/character/CreateCharacter")]
        [HttpPost]
        public IHttpActionResult CreateCharacter([FromBody] Character character)
        {
            Database db = Database.Instance;
            Result test = new Result { Status =  db.InsertCharacter(character)};            

            return Ok(test);            
        }

        [Route("api/character/DeleteCharacter")]
        [HttpPost]
        public IHttpActionResult DeleteCharacter([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());
            Database db = Database.Instance;
            int id = jsonToId.Id;
            Result result = new Result { Status = db.DeleteCharacter(id) };

            return Ok(result);
        }

        [Route("api/character/DeleteStat")]
        [HttpPost]
        public IHttpActionResult DeleteStat([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());
            Database db = Database.Instance;
            int id = jsonToId.Id;
            String statName = jsonToId.StatName;
            Result result = new Result { Status = db.DeleteStat(id, statName) };

            return Ok(result);
        }

        [Route("api/character/DeleteItem")]
        [HttpPost]
        public IHttpActionResult DeleteItem([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());
            Database db = Database.Instance;
            int id = jsonToId.Id;
            String itemName = jsonToId.ItemName;
            Result result = new Result { Status = db.DeleteItem(id, itemName) };

            return Ok(result);
        }

        [Route("api/character/AddStat")]
        [HttpPost]
        public IHttpActionResult AddStat([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());
            Database db = Database.Instance;
            int id = jsonToId.Id;
            String statName = jsonToId.StatName;
            int statValue = jsonToId.StatValue;
            Stat stat = new Stat { Name = statName, Value = statValue };
            Result result = new Result { Status = db.AddStat(id, stat) };

            return Ok(result);
        }

        [Route("api/character/AddItem")]
        [HttpPost]
        public IHttpActionResult AddItem([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());
            Database db = Database.Instance;
            int id = jsonToId.Id;
            String itemName = jsonToId.ItemName;            
            Result result = new Result { Status = db.AddItem(id, itemName) };

            return Ok(result);
        }

        [Route("api/character/UpdateStat")]
        [HttpPost]
        public IHttpActionResult UpdateStat([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());
            Database db = Database.Instance;
            int id = jsonToId.Id;
            String statName = jsonToId.StatName;
            int statValue = jsonToId.StatValue;
            Stat stat = new Stat { Name = statName, Value = statValue };
            Result result = new Result { Status = db.UpdateStat(id, stat) };

            return Ok(result);
        }

        [Route("api/character/UpdateCharacter")]
        [HttpPost]
        public IHttpActionResult UpdateCharacter([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());
            Database db = Database.Instance;
            int id = jsonToId.Id;            
            String characterName = jsonToId.CharacterName;
            String characterDescription = jsonToId.CharacterDescription;
            String characterClass = jsonToId.CharacterClass;            
            Result result = new Result { Status = db.UpdateCharacter(id, characterName, characterDescription, characterClass) };

            return Ok(result);
        }
    }
}
