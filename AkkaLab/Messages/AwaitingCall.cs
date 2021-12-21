namespace Akka.Messages
{
    public class AwaitingCall : ICall
    {
        public string Phone { get; }

        public AwaitingCall(string phone)
        {
            Phone = phone;
        }
    }
}