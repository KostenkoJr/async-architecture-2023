namespace TaskTracker.Events.Domain;

public record TaskAssignedEvent
{
    public Guid PublicId { get; set; }
    public Guid PublicAssigneeId { get; set; }
    public EventMeta<TaskAssignedEventVersion> EventMeta { get; init; }
};

public enum TaskAssignedEventVersion
{
    V1 = 1
}