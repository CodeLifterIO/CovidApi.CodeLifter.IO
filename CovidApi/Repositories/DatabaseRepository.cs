using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CovidApi.Data;
using CovidApi.Services;
using Microsoft.EntityFrameworkCore;

namespace CovidApi.Repositories
{
    public interface IDatabaseRepository
    {
        Task EnsureMigratedAsync();
        Task EnsureSeedAsync();
        Task GenerateDatabaseBackupAsync();
    }

    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly ILogger<DatabaseRepository> _logger;
        private readonly CovidContext _context;
        private readonly IEnvironmentService _envService;

        public DatabaseRepository(CovidContext context,
                                ILogger<DatabaseRepository> logger,
                                IEnvironmentService envService)
        {
            _context = context;
            _logger = logger;
            _envService = envService;
        }

        public async Task EnsureSeedAsync()
        {

        }

        public async Task EnsureMigratedAsync()
        {
            if (_envService.IsDebug())
            {
                await _context.Database.MigrateAsync();
            }
        }

        public async Task GenerateDatabaseBackupAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC SP_Backup_Database;");
        }
    }
}
