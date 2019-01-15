using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(server.Startup))]

namespace server
{
    public partial class Startup
    {
		private PipeMenager pipeMenager;

		internal PipeMenager PipeMenager
		{
			get
			{
				return pipeMenager;
			}

			set
			{
				pipeMenager = value;
			}
		}

		public void Configuration(IAppBuilder app)
        {
			pipeMenager = PipeMenager.getInstance();
			//pipeMenager.
            ConfigureAuth(app);
        }
    }
}
