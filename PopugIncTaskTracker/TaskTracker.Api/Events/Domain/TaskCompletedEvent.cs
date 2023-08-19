namespace TaskTracker.Events.Domain;

public record TaskCompletedEvent
{
    public Guid PublicId { get; set; }
    public Guid PublicAssigneeId { get; set; }
    public EventMeta<TaskCompletedEventVersion> EventMeta { get; init; }
};

public enum TaskCompletedEventVersion
{
    V1 = 1,
}