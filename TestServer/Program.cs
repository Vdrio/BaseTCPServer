using System;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            StandardTCPServer.TCPServer.SetupServer();
            Console.ReadLine();
        }
    }
}
