using Microsoft.Extensions.DependencyInjection;
using SchemaRegistry.Schemas.Tasks.Assigned;
using SchemaRegistry.Schemas.Tasks.Completed;
using SchemaRegistry.Schemas.Tasks.Created;
using SchemaRegistry.Schemas.Transactions;
using SchemaRegistry.Schemas.Users.Created;

namespace SchemaRegistry;

public static class SchemaRegistryLibrary
{
    public static IServiceCollection AddSchemaRegistryLibrary(this IServiceCollection services)
    {
        services.AddSingleton<IUserCreatedEventSchemaRegistry, UserCreatedEventSchemaRegistry>();
        services.AddSingleton<ITaskCompletedEventSchemaRegistry, TaskCompletedEventSchemaRegistry>();
        services.AddSingleton<ITaskAssignedEventSchemaRegistry, TaskAssignedEventSchemaRegistry>();
        services.AddSingleton<ITaskCreatedEventSchemaRegistry, TaskCreatedEventSchemaRegistry>();
        services.AddSingleton<ITransactionAppliedEventSchemaRegistry, TransactionAppliedEventSchemaRegistry>();
        return services;
    }  
}