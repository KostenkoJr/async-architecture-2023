namespace Accounting.Data.Entities;

public record Transaction
{
    public Guid Id { get; set; }
    public Guid PublicId { get; set; }
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
    public string Description { get; set; }
    public TransactionType TransactionType { get; init; }
    public double Debit { get; set; }
    public double Credit { get; set; }
    public DateTime Date { get; set; }
}

public enum TransactionType
{
    Payment = 1,
    Withdrawal = 2,
    Enrollment = 3
}