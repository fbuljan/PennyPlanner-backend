using System.Runtime.Serialization;

namespace PennyPlanner.Exceptions
{
    [Serializable]
    internal class UserAlreadyExistsException : Exception
    {
        public string? ErrorMessage { get; set; } = default!;
        public UserAlreadyExistsException()
        {
        }

        public UserAlreadyExistsException(string? message) : base(message)
        {
            if (message != null) ErrorMessage = message;
        }

        public UserAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}