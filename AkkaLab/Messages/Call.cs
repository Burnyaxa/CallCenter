namespace Akka.Messages
{
    public class Call : ICall
    {
        public string Phone { get; }

        public Call(string phoneNumber)
        {
            Phone = phoneNumber;
        }
    }
}