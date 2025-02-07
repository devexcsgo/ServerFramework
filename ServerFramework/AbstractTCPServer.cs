using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{

    abstract public class AbstractTCPServer
    {
        public int PORT = 7;

        public AbstractTCPServer(string name, int port)
        {
            PORT = port;
            Console.WriteLine($"Server {name} started on port {PORT}");
        }

        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();
            Console.WriteLine("Server started");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client incoming");
                Console.WriteLine($"remote (ip,port) = ({client.Client.RemoteEndPoint})");

                Task.Run(() =>
                {
                    TcpClient tmpClient = client;
                    HandleClient(client);
                });

            }
        }
        void HandleClient(TcpClient socket)
        {
            // Kan sende en strøm af data
            NetworkStream ns = socket.GetStream();
            // Læser og sender
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            TcpServerWork(reader, writer);

            while (socket.Connected)
            {
                // Continues to read, until line skift (enter)
                string? message = reader.ReadLine();
                // WriteLine make sure there is a line skift.
                Console.WriteLine(message);

                if (message == "stop")
                {
                    socket.Close();
                }

                // In case the user write a useless command
                else
                {
                    writer.WriteLine(message);
                    // Clears all buffers. Remember to use this at the end.
                    writer.Flush();
                }
            }
        }

        // Defines a abstract method
        protected abstract void TcpServerWork(StreamReader reader, StreamWriter writer);

    }
}




