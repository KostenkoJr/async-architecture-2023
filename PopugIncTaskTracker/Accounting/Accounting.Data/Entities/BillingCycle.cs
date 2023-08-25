namespace Accounting.Data.Entities;

public record BillingCycle
{
    public Guid Id { get; set; }
    public Guid PublicId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}