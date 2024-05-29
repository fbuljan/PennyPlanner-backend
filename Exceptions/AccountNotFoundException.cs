using System.Runtime.Serialization;

namespace PennyPlanner.Exceptions
{
    [Serializable]
    public class AccountNotFoundException : Exception
    {
        public int Id { get; }

        public AccountNotFoundException() 
        {
        }

        public AccountNotFoundException(int id)
        {
            Id = id;
        }

        public AccountNotFoundException(string? message) : base(message)
        {
        }

        public AccountNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AccountNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
