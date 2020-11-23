using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CovidApi.Models;
using CovidApi.Repositories;
using CovidApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CovidApi.Infrastructure.Startup
{
    public class GithubDataHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<GithubDataHostedService> _logger;

        //private readonly IDataFileRepository _dataFilesRepository;

        private IGithubService _gitService;

        public GithubDataHostedService(IServiceProvider serviceProvider,
                                       ILogger<GithubDataHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Starting {nameof(GithubDataHostedService)}");

            using (var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                _gitService = services.GetRequiredService<IGithubService>();
                var newFiles = await _gitService.DownloadNewFilesFromGithub();

                foreach (DataFile d in newFiles)
                {
                    _logger.LogDebug($"FileName: {d.FileName}");
                    await _gitService.ParseAndDeleteFile(d);
                }
            }
        }

        // Just return, nothing to clean up
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Stopping {nameof(GithubDataHostedService)}");
            return Task.CompletedTask;
        }
    }
}
