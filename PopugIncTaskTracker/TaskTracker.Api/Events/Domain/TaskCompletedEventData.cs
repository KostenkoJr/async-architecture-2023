using TaskTracker.Data.Models;

namespace TaskTracker.Events.Domain;

public record TaskCompletedEventData
{
    public string Event => EventType.TaskCompleted.ToString();
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Cost { get; set; }
    public double Award { get; set; }
    public Status Status { get; set; }
    public Guid AuthorId { get; set; }
    public Guid AssigneeId { get; set; }
};