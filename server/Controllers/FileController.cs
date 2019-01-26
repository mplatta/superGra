using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace server.Controllers
{
    public class FileController : ApiController
    {
        private static int nextAvatar = 1;

        [Route("api/avatar/Upload")]
        [HttpPost]
        public async Task<IHttpActionResult> PostAvatar([FromUri] string id)
        {
            byte[] file = await Request.Content.ReadAsByteArrayAsync();

            bool upload = true;

            string root = HttpContext.Current.Server.MapPath("~/App_Data/Avatar/");

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            root += nextAvatar + ".jpg";

            Image avatar;

            try
            {
                using (var ms = new MemoryStream(file))
                {
                    avatar = Image.FromStream(ms);
                    avatar.Save(root, ImageFormat.Jpeg);
                    nextAvatar++;                    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception caught in process: {0}", ex);
                upload = false;
            }

            JObject result = new JObject();
            result.Add("Status", upload);

            if (upload)
            {
                JObject q = new JObject();
                q.Add("Action", 7);
                q.Add("Id", id);
                q.Add("Path", root);
                QueueController.queue[QueueController.GM].Enqueue(q);
            }

            return Ok(result);
        }

        [Route("api/map/Download")]
        [HttpPost]
        public IHttpActionResult GetMap([FromBody] JObject json)
        {           
            dynamic jsonToPath = JsonConvert.DeserializeObject(json.ToString());
            string path = jsonToPath.Path;            
            byte[] dataBytes = File.ReadAllBytes(path);
            MemoryStream dataStream = new MemoryStream(dataBytes);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(dataStream)
            };
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Map.jpeg"
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            ResponseMessageResult response = ResponseMessage(result);

            return response;
        }        
    }
}
