using TaskTracker.Data.Models;

namespace TaskTracker.Consumer.Events;

public record UserCreatedEvent(Guid Id, string Name, Role Role, string Email);