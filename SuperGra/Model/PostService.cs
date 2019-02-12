using System;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;

namespace SuperGra.Model
{
	public class EventString : EventArgs
	{
		public string JsonString { get; set; }
	}

	class PostService
	{
		public static readonly string API_GET_QUEUE        = "api/queue/GetQueue";
		public static readonly string API_ADD_QUEUE        = "api/queue/AddQueue";
		public static readonly string API_UPDATE_CHARACTER = "api/character/Update";
		public static readonly string IP_ADRESS            = "http://localhost:34450/";

		private static bool _is_start;
		private static int  _sleep_time;

		public EventHandler<EventString> es;

		#region Public api

		public void Start()
		{
			_is_start = true;

			Thread thread = new Thread(_threadLoop);
			thread.Start();
		}

		public void Stop()
		{
			_is_start = false;
		}

		public string GetNews()
		{
			string message = "{\"Id\":\"GameMaster\"}";

			return _post_request(IP_ADRESS + API_GET_QUEUE, message);
		}

		public bool SendNews(string jsonString)
		{
			string result = _post_request(IP_ADRESS + API_ADD_QUEUE, jsonString);
			dynamic success;

			if (result != null)
			{
				success = JsonConvert.DeserializeObject(result);

				return success.Status;
			}

			return false;
		}

		public string SendGet(string url)
		{
			WebRequest request = WebRequest.Create(url);
			request.Credentials = CredentialCache.DefaultCredentials;

			try
			{
				WebResponse response = request.GetResponse();

				Stream dataStream = response.GetResponseStream();
				StreamReader reader = new StreamReader(dataStream);

				string result = reader.ReadToEnd();

				reader.Close();
				response.Close();

				return result;
			}
			catch
			{
				return null;
			}
		}

		public string SendPost(string url, string message)
		{
			string result = _post_request(url, message);
			return result;
		}

		#endregion

		#region Private

		private string _post_request(string adress, string message)
		{
			string result;

			WebRequest request = WebRequest.Create(adress);
			request.Method = "POST";
			request.ContentType = "application/json";

			byte[] byteArray = Encoding.UTF8.GetBytes(message);
			
			request.ContentLength = byteArray.Length;

			Stream dataStream = request.GetRequestStream();

			dataStream.Write(byteArray, 0, byteArray.Length);
			dataStream.Close();

			WebResponse response;
			StreamReader reader;

			try
			{
				response = request.GetResponse();
				dataStream = response.GetResponseStream();

				reader = new StreamReader(dataStream);
				result = reader.ReadToEnd();

				response.Close();
				reader.Close();
				dataStream.Close();
			}
			catch (Exception e)
			{
				result = null;
			}

			return result;
		}

		private void _threadLoop()
		{
			while (_is_start)
			{
				string strResult = GetNews();
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

		#endregion

		#region Constructor

		public PostService()
		{
			_sleep_time = 500;
		}

		#endregion
	}
}
