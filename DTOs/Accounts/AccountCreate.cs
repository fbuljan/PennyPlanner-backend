namespace PennyPlanner.DTOs.Accounts
{
    public class AccountCreate
    {
        public int UserId { get; set; }
        public string Name { get; set; } = default!;
        public float? Balance { get; set; }
        public string? Description { get; set; }
    }
}
