namespace Accounting.Data.Entities;

public record Account
{
    public Guid Id { get; set; }
    public Guid PublicId { get; set; }
    public double Balance { get; set; }
    public Guid UserId { get; set; }
    public Guid PublicUserId { get; set; }
    public User User { get; set; }
    
    public IReadOnlyCollection<Transaction> Transactions { get; set; }
}