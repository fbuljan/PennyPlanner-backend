using System.Runtime.Serialization;

namespace PennyPlanner.Exceptions
{
    [Serializable]
    public class TransactionNotFoundException : Exception
    {
        public int Id { get; }

        public TransactionNotFoundException()
        {
        }

        public TransactionNotFoundException(int id)
        {
            Id = id;
        }

        public TransactionNotFoundException(string? message) : base(message)
        {
        }

        public TransactionNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TransactionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
