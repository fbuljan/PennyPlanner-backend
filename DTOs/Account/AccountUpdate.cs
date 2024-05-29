namespace PennyPlanner.DTOs.Account
{
    public class AccountUpdate
    {
        public int Id { get; set; }
        public string? Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
