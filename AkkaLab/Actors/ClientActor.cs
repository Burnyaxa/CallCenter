using System;
using System.Security.Cryptography;
using System.Threading;
using Akka.Actor;
using Akka.Messages;

namespace Akka.Actors
{
    public class ClientActor : ReceiveActor
    {
        public string PhoneNumber { get; }
        private readonly IActorRef _target;
        private readonly int _waitingPeriod;
        private readonly int _callDuration;
        private readonly int _delay;
        private bool _isCallAccepted;

        public ClientActor(IActorRef target, string phone, int delay)
        {
            _target = target;
            _waitingPeriod = RandomNumberGenerator.GetInt32(1000, 4000);
            _callDuration = RandomNumberGenerator.GetInt32(1000, 5000);
            _delay = delay;
            PhoneNumber = phone;

            Receive<AcceptedCall>(msg =>
            {
                _isCallAccepted = true;
                Console.WriteLine($"Accepted call from {PhoneNumber} by operator {msg.OperatorId}. Talk duration {_callDuration}");
                Thread.Sleep(_callDuration);
                Console.WriteLine($"Call from {PhoneNumber} has ended");
                Sender.Tell(new CallHangUp(PhoneNumber));
            });
            Receive<AwaitingCall>(msg =>
            {
                Console.WriteLine($"Call from {PhoneNumber} is awaiting to be answered. Hang up after {_waitingPeriod}");
                Thread.Sleep(_waitingPeriod);
                if (!_isCallAccepted)
                {
                    Console.WriteLine($"The waiting is over for {PhoneNumber}. Hanging up");
                    Sender.Tell(new AwaitingCallHangUp(PhoneNumber));
                }
            });
            
            Start();
        }

        private void Start()
        {
            Thread.Sleep(_delay);
            Console.WriteLine($"Calling from {PhoneNumber}");
            _target.Tell(new Call(PhoneNumber));
        }
    }
}