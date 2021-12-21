using Akka.Messages;

namespace Akka.States
{
    public class OperatorState
    {
        private Call _call;
        public string Id { get; }

        public Call Call
        {
            get => _call;
            set
            {
                _call = value;
                IsBusy = _call is not null;
            }
        }

        public bool IsBusy { get; private set; }

        public OperatorState(string id)
        {
            Id = id;
        }

        public OperatorState(string id, Call call)
        {
            Id = id;
            Call = call;
        }
    }
}