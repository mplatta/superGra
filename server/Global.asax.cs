﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
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
            Database db = Database.Instance;
            db.Start();

            Thread thread = new Thread(AsynchronousSocketListener.StartListening);
            thread.Start();

            AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);            
        }        

    }
}
