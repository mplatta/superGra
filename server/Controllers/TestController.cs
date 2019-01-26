using Newtonsoft.Json.Linq;
using System.Web.Http;

namespace server.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Test()
        {           
            JObject result = new JObject();
            result.Add("Status", true);

            return Ok(result);
        }        
    }
}
