using Accounting.Api.Settings;
using AccountingScheduler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


public static class Program
{
   public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }
    
    
    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.Configure<KafkaSettings>(configuration.GetSection("Kafka"));
                services.AddHostedService<BillingCycleManager>();
            });
    }
}