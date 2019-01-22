﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace server.Models
{
    public class StateObject
    {
        public Socket workSocket = null;         
        public const int BufferSize = 256;         
        public byte[] buffer = new byte[BufferSize];        
        public StringBuilder sb = new StringBuilder();
    }
}