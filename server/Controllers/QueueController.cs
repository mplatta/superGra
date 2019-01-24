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
    public class QueueController : ApiController
    {
        public static Dictionary<string, Queue<JObject>> queue = new Dictionary<string, Queue<JObject>>();

        [Route("api/queue/GetQueue")]
        [HttpPost]
        public IHttpActionResult GetQueue([FromBody] JObject json)
        {
            dynamic jsonToId = JsonConvert.DeserializeObject(json.ToString());
            String id = jsonToId.Id;

            if (!queue.ContainsKey(id))
            {
                queue.Add(id, new Queue<JObject>());
                json.Add("Action", 1);
                queue["GameMaster"].Enqueue(json);
            }

            JObject result = new JObject();
            if(queue[id].Count==0)
            {
                result.Add("Action", 0);
            }
            else
            {
                result = queue[id].Dequeue();
            }

            return Ok(result);
        }

        [Route("api/queue/AddQueue")]
        [HttpPost]
        public IHttpActionResult AddQueue(JObject json)
        {
            bool added = true;
            dynamic jsonToQueue = JsonConvert.DeserializeObject(json.ToString());
            String id = jsonToQueue.Id;

            if (queue.ContainsKey(id))
            {
                queue[id].Enqueue(json);
            }
            else
            {
                added = false;
            }

            JObject result = new JObject();
            result.Add("Status", added);

            return Ok(result);
        }
    }
}
