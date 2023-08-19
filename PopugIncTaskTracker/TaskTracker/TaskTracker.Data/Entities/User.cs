namespace TaskTracker.Data.Entities;

public class User
{
    public Guid Id { get; set; }
    public Guid PublicId { get; set; }
    public string Name { get; set; }
    public Role Role { get; set; }
    public string Email { get; set; }
}

public enum Role
{
    Admin = 1,
    Worker = 2,
    Manager = 3,
    Accountant = 4
}