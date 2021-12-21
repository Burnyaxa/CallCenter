namespace Akka.Messages
{
    public class AwaitingCallHangUp : ICall
    {
        public string Phone { get; }

        public AwaitingCallHangUp(string phone)
        {
            Phone = phone;
        }
    }
}