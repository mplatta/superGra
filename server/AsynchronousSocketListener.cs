using Newtonsoft.Json;
using server.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace server
{
    public class AsynchronousSocketListener
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private static Dictionary<int, Socket> handlers = new Dictionary<int, Socket>();
        private static int nextId = 0;

        public AsynchronousSocketListener()
        {
        }

        public static void StartListening()
        {           
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 11000);


            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    allDone.Reset();                    
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }

        }

        public static void AcceptCallback(IAsyncResult ar)
        {

            allDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));


                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    Actions(content, state);
                    state.sb.Clear();
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
                else
                {
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }


            }
        }

        private static void Send(Socket handler, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;                
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        private static void Actions(String content, StateObject state)
        {
            Socket handler = state.workSocket;

            content = content.Remove(content.Length - 5);

            dynamic json = JsonConvert.DeserializeObject(content);
            int action = json.Action;

            String data = String.Empty;
            int from;
            int to;
            int roll;

            switch (action)
            {
                case 0:
                    handlers.Add(nextId, handler);

                    data = "{'Action':0, 'YourId':" + nextId + "}";
                    Send(handler, data);

                    if (nextId != 0)
                    {
                        data = "{'Action':0, 'NewId':" + nextId + "}";
                        try
                        {
                            Send(handlers[0], data);
                        }
                        catch
                        {
                            Disconnect(0);
                        }
                    }

                    nextId++;
                    break;
                case 2:
                    from = json.From;
                    int characterId = json.CharacterId;

                    data = "{'Action':2, 'CharacterId':" + characterId + ", 'From':" + from + "}";
                    Send(handlers[0], data);

                    break;
                case 3:
                    to = json.To;

                    data = "{'Action':3}";
                    try
                    {
                        Send(handlers[to], data);
                    }
                    catch
                    {
                        Disconnect(to);
                    }

                    break;
                case 4:
                    from = json.From;
                    to = json.To;
                    roll = json.Roll;

                    data = "{'Action':4, 'Roll':" + roll + ", 'From':" + from + "}";
                    try
                    {
                        Send(handlers[to], data);
                    }
                    catch
                    {
                        Disconnect(to);
                    }
                    break;
                case 5:
                    from = json.From;                    
                    roll = json.Roll;

                    data = "{'Action':4, 'Roll':" + roll + ", 'From':" + from + "}";
                    foreach (KeyValuePair<int, Socket> entry in handlers)
                    {
                        try
                        {
                            Send(entry.Value, data);

                        }
                        catch
                        {
                            Disconnect(entry.Key);
                        }
                    }
                    break;
                case 6:
                    from = json.From;
                    to = json.To;
                    String message = json.Message;

                    data = "{'Action':6, 'From':" + from + ", 'Message':'" + message + "'}";                   
                    try
                    {
                        Send(handlers[to], data);

                    }
                    catch
                    {
                        Disconnect(to);
                    }
                    break;                
                default:                   
                    break;
            }           
        }

        private static void Disconnect(int id)
        {
            String data = String.Empty;
            try
            {
                data = "{'Action':1, 'DisconnectedId':" + id + "}";
                Send(handlers[0], data);
                handlers.Remove(id);
            }
            catch
            {
                foreach(Socket socket in handlers.Values)
                {
                    try
                    {
                        data = "{'Action':1}";
                        Send(socket, data);
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}