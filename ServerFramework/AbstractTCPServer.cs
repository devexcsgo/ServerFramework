using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
    /// <summary>
    /// AbstractTCPServer is an abstract class that provides a framework for creating TCP servers.
    /// It handles client connections and delegates the specific work to be done with each client
    /// to the derived classes through the abstract method TcpServerWork.
    /// </summary>
    abstract public class AbstractTCPServer
    {
        /// <summary>
        /// The port number on which the server listens for incoming connections.
        /// </summary>
        public int PORT = 7;

        /// <summary>
        /// Initializes a new instance of the AbstractTCPServer class with the specified name and port.
        /// </summary>
        /// <param name="name">The name of the server.</param>
        /// <param name="port">The port number on which the server listens for incoming connections.</param>
        public AbstractTCPServer(string name, int port)
        {
            PORT = port;
            Console.WriteLine($"Server {name} started on port {PORT}");
        }

        /// <summary>
        /// Starts the server and begins listening for incoming client connections.
        /// </summary>
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

        /// <summary>
        /// Handles the client connection by setting up the network stream, reader, and writer,
        /// and then calling the abstract method TcpServerWork to perform specific work with the client.
        /// </summary>
        /// <param name="socket">The TcpClient representing the client connection.</param>
        void HandleClient(TcpClient socket)
        {
            // Streaming data
            NetworkStream ns = socket.GetStream();
            // Reader and Writer
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            // Call the abstract method to perform specific work with the client
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
                else
                {
                    // In case the user writes a useless command
                    writer.WriteLine(message);
                    // Clears all buffers. Remember to use this at the end.
                    writer.Flush();
                }
            }
        }

        /// <summary>
        /// An abstract method that must be overridden in derived classes to define the specific work
        /// to be done with each client connection.
        /// </summary>
        /// <param name="reader">The StreamReader for reading data from the client.</param>
        /// <param name="writer">The StreamWriter for writing data to the client.</param>
        protected abstract void TcpServerWork(StreamReader reader, StreamWriter writer);
    }
}