using Auth.Api.Entities;

namespace Auth.Api.Events;

public record UserCreatedEventData(Guid PublicId, string Name, Role Role, string Email);