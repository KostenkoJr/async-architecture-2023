namespace Auth.Api.Models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Role Role { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}

public enum Role
{
    Admin = 1,
    Worker = 2
}