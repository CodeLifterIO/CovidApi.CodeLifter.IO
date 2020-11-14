using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CovidApi.Data;
using CovidApi.Services;
using Microsoft.EntityFrameworkCore;

namespace CovidApi.Data.Repositories
{
    public interface IDatabaseRepository
    {
        void EnsureSeed();
        void EnsureMigrated();
        Task EnsureMigratedAsync();
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

        public void EnsureSeed()
        {

        }


        public void EnsureMigrated()
        {
            if (_envService.IsDebug())
            {
                _context.Database.Migrate();
            }
        }

        public async Task EnsureMigratedAsync()
        {
            if (_envService.IsDebug())
            {
                await _context.Database.MigrateAsync();
            }
        }
    }
}
