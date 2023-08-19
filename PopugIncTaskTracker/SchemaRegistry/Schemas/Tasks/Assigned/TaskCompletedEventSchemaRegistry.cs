using System.Reflection;
using NJsonSchema;

namespace SchemaRegistry.Schemas.Tasks.Assigned;

public interface ITaskAssignedEventSchemaRegistry
{
    Task<string> GetSchemaByVersion(string version);
}

public class TaskAssignedEventSchemaRegistry : ITaskAssignedEventSchemaRegistry
{
    public async Task<string> GetSchemaByVersion(string version)
    {
        var basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Schemas", "Tasks", "Assigned");

        return version switch
        {
            "V1" => (await JsonSchema.FromFileAsync(Path.Combine(basePath, "task_assigned_event_V1.json"))).ToJson(),
            _ => throw new InvalidOperationException("Invalid schema version"),
        };
    }    
}
