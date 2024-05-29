namespace PennyPlanner.DTOs.Account
{
    public class AccountCreate
    {
        public int UserId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public float? Balance { get; set; }
        public string? Description { get; set; }
    }
}
