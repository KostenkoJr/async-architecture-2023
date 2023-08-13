namespace TaskTracker.Data.Models;

public class Task
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Cost { get; set; }
    public double Award { get; set; }
    public Status Status { get; set; }
    
    public Guid AuthorId { get; set; }
    public User Author { get; set; }
    
    public Guid AssigneeId { get; set; }
    public User Assignee { get; set; }
}

public enum Status
{
    InProgress = 1,
    Completed = 2
}