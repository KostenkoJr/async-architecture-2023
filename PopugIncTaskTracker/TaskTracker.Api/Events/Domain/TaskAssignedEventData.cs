namespace TaskTracker.Events.Domain;

public record TaskAssignedEventData
{
    public string Event => EventType.TaskAssigned.ToString();
    public Guid PublicId { get; set; }
    public Guid PublicAssigneeId { get; set; }
};