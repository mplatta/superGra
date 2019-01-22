﻿using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace server.Controllers
{
    public class DnsController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetDns(){

            DNS dns = new DNS { Dns = Dns.GetHostName() };

            return Ok(dns);
        }
    }
}