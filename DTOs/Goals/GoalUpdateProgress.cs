using PennyPlanner.Models;

namespace PennyPlanner.DTOs.Goals
{
    public class GoalUpdateProgress
    {
        public Models.User User { get; set; } = default!;
        public Account Account { get; set; } = default!;
        public Transaction Transaction { get; set; } = default!;
        public float Amount { get; set; }
    }
}
