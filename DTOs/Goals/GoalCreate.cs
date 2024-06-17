using PennyPlanner.Enums;

namespace PennyPlanner.DTOs.Goals
{
    public class GoalCreate
    {
        public string Name { get; set; } = default!;
        public GoalType GoalType { get; set; }
        public DateTime? EndDate { get; set; }
        public float TargetValue { get; set; }
        public int UserId { get; set; }
        public int? AccountId { get; set; }
    }
}
