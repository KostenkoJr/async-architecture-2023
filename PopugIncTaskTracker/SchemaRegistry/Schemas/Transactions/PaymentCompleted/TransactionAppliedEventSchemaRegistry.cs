using System.Reflection;
using NJsonSchema;

namespace SchemaRegistry.Schemas.Transactions.PaymentCompleted;

public interface IPaymentCompletedEventSchemaRegistry
{
    Task<string> GetSchemaByVersion(string version);
}

public class PaymentCompletedEventSchemaRegistry : IPaymentCompletedEventSchemaRegistry
{
    public async Task<string> GetSchemaByVersion(string version)
    {
        var basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Schemas", "Transactions", "PaymentCompleted");

        return version switch
        {
            "V1" => (await JsonSchema.FromFileAsync(Path.Combine(basePath, "payment_completed_event_V1.json"))).ToJson(),
            _ => throw new InvalidOperationException("Invalid schema version"),
        };
    }    
}
