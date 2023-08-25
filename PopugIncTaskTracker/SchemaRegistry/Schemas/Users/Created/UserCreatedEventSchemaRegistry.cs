using System.Reflection;
using NJsonSchema;

namespace SchemaRegistry.Schemas.Users.Created;

public interface IUserCreatedEventSchemaRegistry
{
    Task<string> GetSchemaByVersion(string version);
}

public class UserCreatedEventSchemaRegistry : IUserCreatedEventSchemaRegistry
{
    public async Task<string> GetSchemaByVersion(string version)
    {
        var basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Schemas", "Users", "Created");

        return version switch
        {
            "V1" => (await JsonSchema.FromFileAsync(Path.Combine(basePath, "user_created_event_V1.json"))).ToJson(),
            _ => throw new InvalidOperationException("Invalid schema version"),
        };
    }    
}
