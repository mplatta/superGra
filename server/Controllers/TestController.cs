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
        [HttpGet]
        public IHttpActionResult Test()
        {
            //TODO
            //Sprawdzić czy łaczy się z aplikacją
            Test test = new Test { Status = true };
            return Ok(test);
        }
    }
}
