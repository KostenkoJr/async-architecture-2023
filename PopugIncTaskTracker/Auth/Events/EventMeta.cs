namespace Auth.Api.Events;

public record EventMeta<TEventVersion>
{
    public Guid Id { get; init; }
    public TEventVersion Version { get; init; }
    public string Name { get; init; }
    public DateTime Time { get; init; }
    public string Producer { get; init; }
}
