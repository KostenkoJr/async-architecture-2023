using System.Reflection;
using NJsonSchema;

namespace SchemaRegistry.Schemas.Tasks.Completed;

public interface ITaskCompletedEventSchemaRegistry
{
    Task<string> GetSchemaByVersion(string version);
}

public class TaskCompletedEventSchemaRegistry : ITaskCompletedEventSchemaRegistry
{
    public async Task<string> GetSchemaByVersion(string version)
    {
        var basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Schemas", "Tasks", "Completed");

        return version switch
        {
            "V1" => (await JsonSchema.FromFileAsync(Path.Combine(basePath, "task_completed_event_V1.json"))).ToJson(),
            _ => throw new InvalidOperationException("Invalid schema version"),
        };
    }    
}
