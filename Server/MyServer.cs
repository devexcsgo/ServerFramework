using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ServerFramework;

namespace Server
{
    public class MyServer : AbstractTCPServer
    {
        public MyServer(string name, int port) : base(name, port)
        {
        }

        protected override void TcpServerWork(StreamReader reader, StreamWriter writer)
        {
            writer.WriteLine("Hello, World!");
            writer.Flush();
        }
    }
}