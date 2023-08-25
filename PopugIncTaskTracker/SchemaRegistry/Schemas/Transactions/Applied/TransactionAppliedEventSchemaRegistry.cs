using System.Reflection;
using NJsonSchema;

namespace SchemaRegistry.Schemas.Transactions;

public interface ITransactionAppliedEventSchemaRegistry
{
    Task<string> GetSchemaByVersion(string version);
}

public class TransactionAppliedEventSchemaRegistry : ITransactionAppliedEventSchemaRegistry
{
    public async Task<string> GetSchemaByVersion(string version)
    {
        var basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Schemas", "Transactions", "Applied");

        return version switch
        {
            "V1" => (await JsonSchema.FromFileAsync(Path.Combine(basePath, "transaction_applied_event_V1.json"))).ToJson(),
            _ => throw new InvalidOperationException("Invalid schema version"),
        };
    }    
}
