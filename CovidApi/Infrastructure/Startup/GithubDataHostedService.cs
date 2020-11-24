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

        private IDataFileRepository _dataFileRepository;
        private IDataUpdateRepository _dataUpdateRepository;

        private IGithubService _gitService;

        public GithubDataHostedService(IServiceProvider serviceProvider,
                                       ILogger<GithubDataHostedService> logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                _gitService = services.GetRequiredService<IGithubService>();
                _dataUpdateRepository = services.GetRequiredService<IDataUpdateRepository>();
                _dataFileRepository = services.GetRequiredService<IDataFileRepository>();

                var newFiles = await _gitService.DownloadNewFilesFromGithub();

                DataUpdate update = new DataUpdate();
                if (newFiles.Count > 0) update.StartFileName = newFiles[0].FileName;

                foreach (DataFile d in newFiles)
                {
                    _logger.LogDebug($"FileName: {d.FileName}");
                    await _gitService.ParseAndDeleteFile(d);

                    d.CompletedAt = DateTime.UtcNow;
                    d.Completed = true;
                    await _dataFileRepository.AddAsync(d);

                    update.LastCompletedFileName = d.FileName;
                    update.RecordsProcessed += (int)d.RecordsProcessed;
                }

                update.CompletedAt = DateTime.UtcNow;
                update.Completed = true;

                await _dataUpdateRepository.AddAsync(update);
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
