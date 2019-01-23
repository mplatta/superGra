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
            JObject result = new JObject();
            result.Add("Status", true);

            return Ok(result);
        }        
    }
}
