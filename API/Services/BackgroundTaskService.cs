using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Services
{
    public class BackgroundTaskService : IHostedService, IDisposable
    {
        private readonly ILogger<BackgroundTaskService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;
        private string _status;
        private DateTimeOffset _lastRun;

        public BackgroundTaskService(ILogger<BackgroundTaskService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _status = "Not started";
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background Task Service is starting.");
            _status = "Running";

            // Schedule the background task to run every minute
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background Task Service is stopping.");
            _status = "Stopped";

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation("Background Task Service is working at {time}.", DateTimeOffset.Now);
            _lastRun = DateTimeOffset.Now;

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var transactionService = scope.ServiceProvider.GetRequiredService<TransactionService>();

                var transaction = await context.Transactions.Where(t => t.Related_Deal_Id == 2).FirstOrDefaultAsync();

                if (transaction != null)
                {
                    var updatedTransaction = await transactionService.AccruedValues(transaction);
                    // Process the updated transaction as needed
                }
            }

            //_logger.LogInformation("Testing");
            
            PerformBackgroundTask();
        }

        private void PerformBackgroundTask()
        {
            // Example task: Log current time
            _logger.LogInformation("Performing background task at: {time}", DateTimeOffset.Now);
            _status = $"Last run at {_lastRun}";
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public string GetStatus()
        {
            return _status;
        }

        public DateTimeOffset GetLastRun()
        {
            return _lastRun;
        }
    }
}
