namespace PennyPlanner.DTOs.Goals
{
    public class GoalUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime? EndDate { get; set; }
        public float? TargetValue { get; set; }
        public float? CurrentValue { get; set; }
        public bool? IsAchieved { get; set; }
    }
}
