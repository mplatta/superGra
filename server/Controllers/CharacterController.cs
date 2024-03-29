﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using server.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace server.Controllers
{
    public class CharacterController : ApiController
    {        

        [HttpGet]
        public IEnumerable<Character> GetAllCharacters()
        {
            Database db = new Database();
            List<Character> characters = db.GetCharacters();

            return characters;
        }

        [HttpGet]
        public IHttpActionResult GetCharacter(int id)
        {
            Database db = new Database();
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
            Database db = new Database();

            JObject result = db.InsertCharacter(character);            

            dynamic json = JsonConvert.DeserializeObject(result.ToString());

            bool add = json.Status;
            int id = json.Id;

            if (add)
            {
                JObject q = new JObject();
                q.Add("Action", 2);
                q.Add("CharacterId", id);

                QueueController.queue[QueueController.GM].Enqueue(q);
            }

            return Ok(result);            
        }

        [Route("api/character/DeleteCharacter")]
        [HttpPost]
        public IHttpActionResult DeleteCharacter([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());
            Database db = new Database();
            int id = jsonToId.Id;           

            JObject result = new JObject();
            result.Add("Status", db.DeleteCharacter(id));

            return Ok(result);
        }

        [Route("api/character/DeleteStat")]
        [HttpPost]
        public IHttpActionResult DeleteStat([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());
            Database db = new Database();
            int id = jsonToId.Id;
            String statName = jsonToId.StatName;            

            JObject result = new JObject();
            result.Add("Status", db.DeleteStat(id, statName));

            return Ok(result);
        }

        [Route("api/character/DeleteItem")]
        [HttpPost]
        public IHttpActionResult DeleteItem([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());
            Database db = new Database();
            int id = jsonToId.Id;
            String itemName = jsonToId.ItemName;
            
            JObject result = new JObject();
            result.Add("Status", db.DeleteItem(id, itemName));

            return Ok(result);
        }

        [Route("api/character/AddStat")]
        [HttpPost]
        public IHttpActionResult AddStat([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());
            Database db = new Database();
            int id = jsonToId.Id;
            String statName = jsonToId.StatName;
            int statValue = jsonToId.StatValue;
            Stat stat = new Stat { Name = statName, Value = statValue };
            
            JObject result = new JObject();
            result.Add("Status", db.AddStat(id, stat));

            return Ok(result);
        }

        [Route("api/character/AddItem")]
        [HttpPost]
        public IHttpActionResult AddItem([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());
            Database db = new Database();
            int id = jsonToId.Id;
            String itemName = jsonToId.ItemName;            
            
            JObject result = new JObject();
            result.Add("Status", db.AddItem(id, itemName));

            return Ok(result);
        }

        [Route("api/character/UpdateStat")]
        [HttpPost]
        public IHttpActionResult UpdateStat([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());
            Database db = new Database();
            int id = jsonToId.Id;
            String statName = jsonToId.StatName;
            int statValue = jsonToId.StatValue;
            Stat stat = new Stat { Name = statName, Value = statValue };
           
            JObject result = new JObject();
            result.Add("Status", db.UpdateStat(id, stat));

            return Ok(result);
        }

        [Route("api/character/UpdateCharacter")]
        [HttpPost]
        public IHttpActionResult UpdateCharacter([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());
            Database db = new Database();
            int id = jsonToId.Id;            
            String characterName = jsonToId.CharacterName;
            String characterDescription = jsonToId.CharacterDescription;
            String characterClass = jsonToId.CharacterClass;            
            
            JObject result = new JObject();
            result.Add("Status", db.UpdateCharacter(id, characterName, characterDescription, characterClass));

            return Ok(result);
        }

        [Route("api/character/Update")]
        [HttpPost]
        public IHttpActionResult Update([FromBody] Character character)
        {
            Database db = new Database();
            bool add = db.Update(character);
            JObject result = new JObject();
            result.Add("Status", add);

            if (add && QueueController.queue.ContainsKey(character.Id))
            {
                JObject q = new JObject();
                q.Add("Action", 3);
                q.Add("CharacterId", character.CharacterId);

                QueueController.queue[QueueController.GM].Enqueue(q);               
                QueueController.queue[character.Id].Enqueue(q);
            }                    

            return Ok(result);
        }
    }
}
