using Akka.Actor;

namespace Akka.Actors
{
    public class AwaitingCaller
    {
        public IActorRef Actor { get; }
        public string Phone { get; }

        public AwaitingCaller(IActorRef actor, string phone)
        {
            Actor = actor;
            Phone = phone;
        }
    }
}