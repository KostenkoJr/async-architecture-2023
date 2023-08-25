namespace Accounting.Api.Events;

public class PaymentCompletedEvent
{
    public Guid PublicId { get; init; }
    public double Amount { get; init; }
    public EventMeta<PaymentCompletedEventVersion> EventMeta { get; init; }
}

public enum PaymentCompletedEventVersion
{
    V1 = 1,
}