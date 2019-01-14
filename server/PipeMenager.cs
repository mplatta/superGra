using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace server
{
	class PipeMenager
	{
		private static NamedPipeClientStream clientPipe;
		private static NamedPipeServerStream serverPipe;

		private static StreamWriter streamWriter;
		private static StreamReader streamReader;

		private static PipeMenager _instance = null;

		public static void createClient(string name)
		{
			if (serverPipe == null)
			{
				clientPipe = new NamedPipeClientStream(name);
			}
		}

		public static void createServer(string name)
		{
			if (clientPipe == null)
			{
				serverPipe = new NamedPipeServerStream(name);
			}
		}

		public static void startClient()
		{
			if (clientPipe != null)
			{
				clientPipe.Connect();

				streamWriter = new StreamWriter(clientPipe);
				streamWriter.AutoFlush = true;


			}
		}

		public static void startServer()
		{
			if (serverPipe != null)
			{
				serverPipe.WaitForConnection();
			}
		}

		public static void send(string message)
		{
			if (streamWriter != null)
			{
				streamWriter.WriteLine(message + ";");
			}
		}

		public static string read()
		{
			if (streamReader != null)
			{
				return streamReader.ReadLine();
			}

			return null;
		}

		public static void closeClient()
		{
			if (clientPipe != null)
			{
				clientPipe.Close();
			}
		}

		public static void closeServer()
		{
			if (serverPipe != null)
			{
				serverPipe.Close();
			}
		}

		public static PipeMenager getInstance()
		{
			if (_instance != null)
			{
				return _instance;
			}

			return new PipeMenager();
		}

		private PipeMenager()
		{
			clientPipe = null;
			serverPipe = null;

			streamReader = null;
			streamWriter = null;
		}

		~PipeMenager()
		{
			if (clientPipe != null) clientPipe.Close();
			if (serverPipe != null) serverPipe.Close();
			if (streamReader != null) streamReader.Close();
			if (streamWriter != null) streamWriter.Close();
		}
	}
}