namespace TaskTracker.Data.Entities;

public class Task
{
    public Guid Id { get; set; }
    public Guid PublicId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Cost { get; set; }
    public double Award { get; set; }
    public Status Status { get; set; }
    
    public Guid AuthorId { get; set; }
    public Guid PublicAuthorId { get; set; }
    public User Author { get; set; }
    
    public Guid AssigneeId { get; set; }
    public Guid PublicAssigneeId { get; set; }
    public User Assignee { get; set; }
}

public enum Status
{
    InProgress = 1,
    Completed = 2
}