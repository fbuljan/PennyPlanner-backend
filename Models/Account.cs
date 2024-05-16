namespace PennyPlanner.Models
{
    public class Account : BaseEntity
    {
        public User User { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
