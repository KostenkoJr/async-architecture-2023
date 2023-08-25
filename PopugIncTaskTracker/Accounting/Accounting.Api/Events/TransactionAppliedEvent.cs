using Accounting.Data.Entities;

namespace Accounting.Api.Events;

public class TransactionAppliedEvent
{
    public Guid AccountPublicId { get; init; }
    public TransactionType TransactionType { get; init; }
    public double Credit { get; init; }
    public double Debit { get; init; }
    public EventMeta<TransactionAppliedEventVersion> EventMeta { get; init; }
}

public enum TransactionAppliedEventVersion
{
    V1 = 1,
}