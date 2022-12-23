using Monopoly_server.GameLogic;
using System;

namespace TCPServer
{
    internal class Program
    {
        private static void Main()
        {
            EntityManager.Init();

            Console.Title = "XServer";
            Console.ForegroundColor = ConsoleColor.White;

            var server = new XServer();
            server.Start();
            server.AcceptClients();
        }
    }
}
