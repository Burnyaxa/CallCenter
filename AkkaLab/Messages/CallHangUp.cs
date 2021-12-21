namespace Akka.Messages
{
    public class CallHangUp : ICall
    {
        public string Phone { get; }

        public CallHangUp(string phone)
        {
            Phone = phone;
        }
    }
}