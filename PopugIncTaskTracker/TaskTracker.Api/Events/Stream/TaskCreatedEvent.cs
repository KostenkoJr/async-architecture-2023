namespace TaskTracker.Events.Stream;

public record TaskCreatedEvent
{
    public Guid PublicId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Cost { get; set; }
    public double Award { get; set; }
    public Guid PublicAuthorId { get; set; }
    public Guid PublicAssigneeId { get; set; }
    public EventMeta<TaskCreatedEventVersion> EventMeta { get; init; }
};

public enum TaskCreatedEventVersion
{
    V1 = 1,
}