using Newtonsoft.Json.Linq;
using server.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace server.Controllers
{
    public class FileController : ApiController
    {
        [Route("api/avatar/Upload")]
        [HttpPost]
        public async Task<IHttpActionResult> PostAvatar([FromUri] string fileName)
        {
            byte[] file = await Request.Content.ReadAsByteArrayAsync();

            bool upload = true;

            string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Avatar/");

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            root += fileName;

            try
            {
                using (var fs = new FileStream(root, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(file, 0, file.Length);                    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception caught in process: {0}", ex);
                upload = false;
            }

            JObject result = new JObject();
            result.Add("Status", upload);           

            return Ok(result);
        }

        [Route("api/avatar/Download")]
        [HttpGet]
        public byte[] GetAvatar([FromUri] string fileName)
        {
            string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Avatar/");
            root += fileName;

            byte[] file = File.ReadAllBytes(root);            

            return file;
        }

        [Route("api/map/Upload")]
        [HttpPost]
        public async Task<IHttpActionResult> PostMap([FromUri] string fileName)
        {
            byte[] file = await Request.Content.ReadAsByteArrayAsync();

            bool upload = true;

            string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Map/");

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            root += fileName;

            try
            {
                using (var fs = new FileStream(root, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(file, 0, file.Length);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception caught in process: {0}", ex);
                upload = false;
            }

            JObject result = new JObject();
            result.Add("Status", upload);

            return Ok(result);
        }

        [Route("api/map/Download")]
        [HttpGet]
        public byte[] GetMap([FromUri] string fileName)
        {
            string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Map/");
            root += fileName;

            byte[] file = File.ReadAllBytes(root);

            return file;
        }

    }
}
