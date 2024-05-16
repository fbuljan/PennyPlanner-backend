namespace PennyPlanner.Models
{
    public class Goal : BaseEntity
    {
        public User User { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int DesiredAmount { get; set; }
        public DateTime SetUpDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
