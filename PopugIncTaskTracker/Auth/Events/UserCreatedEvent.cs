using Auth.Api.Entities;

namespace Auth.Api.Events;

public record UserCreatedEvent(Guid PublicId, string Name, Role Role, string Email)
{
    public EventMeta<UserCreatedEventVersion> EventMeta { get; init; }
};

public enum UserCreatedEventVersion
{
    V1 = 1,
}