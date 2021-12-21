using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Actors;

namespace Akka
{
    class Program
    {
        private const int _numberOfClients = 10;

        static void Main(string[] args)
        {
            using var system = ActorSystem.Create("CallCenter");

            var callCenterActor = system.ActorOf<CallCenterActor>("callCenter");
            var clientIndexes = new List<int>();
            for (var i = 0; i < _numberOfClients; i++)
            {
                clientIndexes.Add(i);
            }

            Parallel.ForEach(clientIndexes, index =>
            {
                var delay = (index + 1) * 1000;
                system.ActorOf(Props.Create(() =>
                        new ClientActor(callCenterActor, $"phone{index}", delay)),
                    $"client{index}");
                
            });

            Console.ReadLine();
        }
    }
}