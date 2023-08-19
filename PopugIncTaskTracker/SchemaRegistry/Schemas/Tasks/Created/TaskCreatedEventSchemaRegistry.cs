using System.Reflection;
using NJsonSchema;

namespace SchemaRegistry.Schemas.Tasks.Created;

public interface ITaskCreatedEventSchemaRegistry
{
    Task<string> GetSchemaByVersion(string version);
}

public class TaskCreatedEventSchemaRegistry : ITaskCreatedEventSchemaRegistry
{
    public async Task<string> GetSchemaByVersion(string version)
    {
        var basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Schemas", "Tasks", "Created");

        return version switch
        {
            "V1" => (await JsonSchema.FromFileAsync(Path.Combine(basePath, "task_created_event_V1.json"))).ToJson(),
            _ => throw new InvalidOperationException("Invalid schema version"),
        };
    }    
}
