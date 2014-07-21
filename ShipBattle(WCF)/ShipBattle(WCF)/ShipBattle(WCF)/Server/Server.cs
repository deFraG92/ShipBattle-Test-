using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using GameUtils;

namespace GameServer
{
    class Server
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";
            Uri address = new Uri("net.tcp://localhost:8080/IGameContract");
            NetTcpBinding bind = new NetTcpBinding();
            Type contract = typeof(IGameContract);
            ServiceHost host = new ServiceHost(typeof(GameContract));
            host.AddServiceEndpoint(contract, bind, address);
            host.Open();
            Console.WriteLine("Сервер запущен");
            Console.ReadKey();
            host.Close();
        }
    }
}
