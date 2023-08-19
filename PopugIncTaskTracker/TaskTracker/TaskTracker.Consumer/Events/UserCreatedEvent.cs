using TaskTracker.Data.Entities;

namespace TaskTracker.Consumer.Events;

public record UserCreatedEvent(Guid PublicId, string Name, Role Role, string Email);