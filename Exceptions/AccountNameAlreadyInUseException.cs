using System.Runtime.Serialization;

namespace PennyPlanner.Exceptions
{
    [Serializable]
    public class AccountNameAlreadyInUseException : Exception
    {
        public int? Id { get; }
        public string? Name { get; }

        public AccountNameAlreadyInUseException()
        {
        }

        public AccountNameAlreadyInUseException(int? id, string? name)
        {
            Id = id;
            Name = name;
        }

        public AccountNameAlreadyInUseException(string? message) : base(message)
        {
        }

        public AccountNameAlreadyInUseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AccountNameAlreadyInUseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
