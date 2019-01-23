using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Threading;

namespace SuperGra.Model
{
	public class EventString : EventArgs
	{
		public string JsonString { get; set; }
	}

	class PostService
	{
		private static readonly string apiGetQueue = "api/queue/GetQueue";
		private static readonly string apiAddQueue = "api/queue/AddQueue";
		private static readonly string ipAdress = "http://localhost:34450/";
		//private static readonly string ipAdress = "http://localhost:34450/api/queue/GetQueue";
		//private static readonly string ipAdress = "http://192.168.0.5:34450/api/queue/GetQueue";
		private static bool _isStart = true;

		public EventHandler<EventString> es;

		public void start()
		{
			Thread thread = new Thread(_threadLoop);
			thread.Start();
		}

		private void _threadLoop()
		{
			while (_isStart)
			{
				string strResult = getNews();
				dynamic jsonResult = JsonConvert.DeserializeObject(strResult);
				int action = jsonResult.Action;

				if (0 != action)
				{
					EventString es = new EventString { JsonString = strResult };
					raiseEvent(es);
				}

				Thread.Sleep(500);
			}
		}

		protected virtual void raiseEvent(EventString message)
		{
			EventHandler<EventString> handler = es;
			if(handler != null)
			{
				handler(this, message);
			}
		}

		public void stop()
		{
			_isStart = false;
		}

		public string getNews()
		{
			string result;

			WebRequest request = WebRequest.Create(ipAdress + apiGetQueue);
			request.Method = "POST";

			string postData = "{\"Id\":\"GameMaster\"}";
			byte[] byteArray = Encoding.UTF8.GetBytes(postData);
			request.ContentType = "application/json";
			request.ContentLength = byteArray.Length;
			Stream dataStream = request.GetRequestStream();
			dataStream.Write(byteArray, 0, byteArray.Length);
			dataStream.Close();
			WebResponse response = request.GetResponse();

			dataStream = response.GetResponseStream();
			StreamReader reader = new StreamReader(dataStream);
			result = reader.ReadToEnd();

			reader.Close();
			dataStream.Close();
			response.Close();

			return result;
		}

		public bool sendNews(string jsonString)
		{
			string result;

			WebRequest request = WebRequest.Create(ipAdress + apiAddQueue);
			request.Method = "POST";

			byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);
			request.ContentType = "application/json";
			request.ContentLength = byteArray.Length;
			Stream dataStream = request.GetRequestStream();
			dataStream.Write(byteArray, 0, byteArray.Length);
			dataStream.Close();
			WebResponse response = request.GetResponse();

			dataStream = response.GetResponseStream();
			StreamReader reader = new StreamReader(dataStream);
			result = reader.ReadToEnd();

			reader.Close();
			dataStream.Close();
			response.Close();

			dynamic success = JsonConvert.DeserializeObject(result);

			return success.Status;
		}

		public PostService()
		{
			
		}

	}
}
