using ServerFramework;
using Server;

Console.WriteLine("Hello, World!");

AbstractTCPServer server = new MyServer("serveren", 7007);
server.Start();