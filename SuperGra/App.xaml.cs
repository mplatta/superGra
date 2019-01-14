using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SuperGra
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
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

		private void Application_Startup(object o, StartupEventArgs e)
		{
			pipeMenager = PipeMenager.getInstance();

			MainWindow wnd = new MainWindow();

			wnd.Title = "Super GRA";
			wnd.Show();
		}
	}
}
