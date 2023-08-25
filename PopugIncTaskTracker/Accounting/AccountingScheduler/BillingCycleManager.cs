using Microsoft.Extensions.Hosting;

namespace AccountingScheduler;

public class BillingCycleManager : BackgroundService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(24));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            try
            {
               //close billing cycle
               //produce transaction event
            }
            catch (Exception e)
            {
                //log errror
            }
        } while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested);
    }
    
    
}