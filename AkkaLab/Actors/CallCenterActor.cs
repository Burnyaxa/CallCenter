using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Messages;
using Akka.States;

namespace Akka.Actors
{
    public class CallCenterActor : ReceiveActor
    {
        private readonly List<OperatorState> _operatorStates = new List<OperatorState>()
        {
            new("1"),
            new("2"),
            new("3"),
            new("4"),
            new("5")
        };

        private readonly List<AwaitingCaller> _awaitingCallers = new List<AwaitingCaller>();

        public CallCenterActor()
        {
            Receive<Call>(HandleCall);
            Receive<CallHangUp>(msg =>
            {
                var busyOperator = _operatorStates.FirstOrDefault(x => x.Call?.Phone == msg.Phone);
                busyOperator.Call = null;
                NotifyCallHangUp();
            });
            Receive<AwaitingCallHangUp>(msg =>
            {
                var awaitingCaller = _awaitingCallers.FirstOrDefault(x => x.Phone == msg.Phone);
                if (awaitingCaller != null)
                {
                    _awaitingCallers.Remove(awaitingCaller);
                }
            });
        }

        private void NotifyCallHangUp()
        {
            var caller = _awaitingCallers.FirstOrDefault();
            var availableOperator = _operatorStates.FirstOrDefault(x => !x.IsBusy);
            if (caller != null && availableOperator != null)
            {
                _awaitingCallers.Remove(caller);
                availableOperator.Call = new Call(caller.Phone);
                caller.Actor.Tell(new AcceptedCall(caller.Phone, availableOperator.Id));
            }
        }

        private void HandleCall(Call msg)
        {
            var availableOperator = _operatorStates.FirstOrDefault(x => !x.IsBusy);
            if (availableOperator != null)
            {
                availableOperator.Call = msg;
                var acceptedCall = new AcceptedCall(msg.Phone, availableOperator.Id);
                Sender.Tell(acceptedCall);
            }
            else
            {
                _awaitingCallers.Add(new AwaitingCaller(Sender, msg.Phone));
                Sender.Tell(new AwaitingCall(msg.Phone));
            }
        }
    }
}