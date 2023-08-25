using Accounting.Data.Entities;

namespace Accounting.Consumer;

public record UserCreatedEvent(Guid PublicId, string Name, Role Role, string Email);