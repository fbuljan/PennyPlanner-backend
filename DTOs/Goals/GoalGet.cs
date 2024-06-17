using PennyPlanner.Enums;
using PennyPlanner.Models;

namespace PennyPlanner.DTOs.Goals
{
    public class GoalGet
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public GoalType GoalType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public float TargetValue { get; set; }
        public float CurrentValue { get; set; }
        public bool IsAchieved { get; set; }
        public Account? Account { get; set; }
    }
}
