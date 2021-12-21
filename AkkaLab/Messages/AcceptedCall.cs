using Akka.States;

namespace Akka.Messages
{
    public class AcceptedCall : ICall
    {
        public string OperatorId { get; }
        public string Phone { get; }

        public AcceptedCall(string phone, string operatorId)
        {
            OperatorId = operatorId;
            Phone = phone;
        }
    }
}