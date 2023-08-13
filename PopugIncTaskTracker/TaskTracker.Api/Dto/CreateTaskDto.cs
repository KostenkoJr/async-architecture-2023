namespace TaskTracker.Dto;

public class CreateTaskDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public double Cost { get; set; }
    public double Award { get; set; }
    public Guid AssigneeId { get; set; }
}