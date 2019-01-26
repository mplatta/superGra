using server.Controllers;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace server
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
            LocalDB.CreateLocalDB("Database");

            Database db = new Database();
            db.Start();

            QueueController.queue.Add(QueueController.GM, new Queue<Newtonsoft.Json.Linq.JObject>());            

            //Thread thread = new Thread(AsynchronousSocketListener.StartListening);
            //thread.Start();

            AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);            
        }        

    }
}
