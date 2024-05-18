using System.Runtime.Serialization;

namespace PennyPlanner.Exceptions
{
    [Serializable]
    public class UserNotFoundException : Exception
    {
        public int Id { get; }

        public UserNotFoundException()
        {
        }

        public UserNotFoundException(int id)
        {
            Id = id;
        }

        public UserNotFoundException(string? message) : base(message)
        {
        }

        public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
