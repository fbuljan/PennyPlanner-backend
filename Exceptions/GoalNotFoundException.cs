using System.Runtime.Serialization;

namespace PennyPlanner.Exceptions
{
    [Serializable]
    public class GoalNotFoundException : Exception
    {
        public int Id { get; }

        public GoalNotFoundException()
        {
        }

        public GoalNotFoundException(int id)
        {
            Id = id;
        }

        public GoalNotFoundException(string? message) : base(message)
        {
        }

        public GoalNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected GoalNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
