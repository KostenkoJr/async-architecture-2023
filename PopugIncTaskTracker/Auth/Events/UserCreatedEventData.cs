using Auth.Api.Models;

namespace Auth.Api.Events;

public record UserCreatedEventData(Guid Id, string Name, Role Role, string Email);