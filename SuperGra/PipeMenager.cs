using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.IO;

namespace SuperGra
{
	class PipeMenager
	{
		private NamedPipeClientStream clientPipe;
		private NamedPipeServerStream serverPipe;

		private StreamWriter streamWriter;
		private StreamReader streamReader;

		public void createClient(string name)
		{
			if (serverPipe == null)
			{
				clientPipe = new NamedPipeClientStream(name);
			}
		}

		public void createServer(string name)
		{
			if (clientPipe == null)
			{
				serverPipe = new NamedPipeServerStream(name);
			}
		}

		public void startClient()
		{
			if (clientPipe != null)
			{
				clientPipe.Connect();

				streamWriter = new StreamWriter(clientPipe);
				streamWriter.AutoFlush = true;


			}
		}

		public void startServer()
		{
			if (serverPipe != null)
			{
				serverPipe.WaitForConnection();
			}
		}

		

		public PipeMenager()
		{
			clientPipe = null;
			serverPipe = null;

			streamReader = null;
			streamWriter = null;
		}
	}
}
