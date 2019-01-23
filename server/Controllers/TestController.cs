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
    public class TestController : ApiController
    {
        static public Queue<string> dupa = new Queue<string>();

        [HttpGet]
        public IHttpActionResult Test()
        {
            //TODO
            //Sprawdzić czy łaczy się z aplikacją
            Result test = new Result { Status = true };
            return Ok(test);
        }

        [Route("api/test/testq")]
        [HttpPost]
        public IHttpActionResult TestQ([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());

            String message = jsonToId.Message;

            Result result = new Result { Status = true };
            dupa.Enqueue(message);
            return Ok(json);
        }

        [Route("api/test/testget")]
        [HttpGet]
        public IEnumerable<string> TestQe()
        {

            return dupa;
        }
    }
}
