using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CovidApi.Infrastructure.Startup
{
    public class LoggingTestHostedService : IHostedService
    {
        // We need to inject the IServiceProvider so we can create 
        // the scoped service, FiscalDataDbContext
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LoggingTestHostedService> _logger;

        public LoggingTestHostedService(
            IServiceProvider serviceProvider,
            ILogger<LoggingTestHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            int count = 0;
            _logger.LogDebug($"Starting {nameof(LoggingTestHostedService)}");

            // Create a new scope to retrieve scoped services, as the DbContext class
            // is registered as "scoped"
            using (var scope = _serviceProvider.CreateScope())
            {
                while(count < 20)
                {
                    count++;
                    await Task.Run(async () =>
                    {
                        await Task.Delay(333);
                        _logger.LogDebug($"{nameof(LoggingTestHostedService)} LOOP: {count}");
                    });
                    

                }
            }
        }

        // Just return, nothing to clean up
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Stopping {nameof(LoggingTestHostedService)}");
            return Task.CompletedTask;
        }
    }
}
