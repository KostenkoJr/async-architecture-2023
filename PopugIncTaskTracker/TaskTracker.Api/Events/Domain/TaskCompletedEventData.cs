namespace TaskTracker.Events.Domain;

public record TaskCompletedEventData
{
    public string Event => EventType.TaskCompleted.ToString();
    public Guid PublicId { get; set; }
    public Guid PublicAssigneeId { get; set; }
};