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
        //TODO
        //Usunąć testowanie
        Character[] characters = new Character[]
        {
            new Character { Id = 1, Name = "Test1", Description = "Test1", Class = "AAA", Stats = new Dictionary<string, int>(), Equipment = new List<string>()},
            new Character { Id = 2, Name = "Test11", Description = "Test2", Class = "BBB", Stats = new Dictionary<string, int>(), Equipment = new List<string>()},
            new Character { Id = 3, Name = "Test111", Description = "Test3", Class = "CCC", Stats = new Dictionary<string, int>(), Equipment = new List<string>()}
        };

        [HttpGet]
        public IEnumerable<Character> GetAllCharacters()
        {            
            characters[0].Stats.Add("AGI", 59);
            characters[0].Stats.Add("HP", 52);

            characters[0].Equipment.Add("Sword");
            characters[0].Equipment.Add("Axe");

            return characters;
        }

        [HttpGet]
        public IHttpActionResult GetCharacter(int id)
        {
            var product = characters.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IHttpActionResult CreateCharacter([FromBody] Character character)
        {
            Database db = Database.Instance;
            Test test = new Test { Status =  db.InsertCharacter(character)};            

            return Ok(test);            
        }
    }
}
