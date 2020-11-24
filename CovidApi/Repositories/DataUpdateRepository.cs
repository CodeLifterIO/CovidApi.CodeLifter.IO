using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidApi.Data;
using CovidApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CovidApi.Repositories
{
    public interface IDataUpdateRepository
    {
        Task AddAsync(DataUpdate dUpdate);
        Task<List<DataUpdate>> GetAllAsync();
        Task<DataUpdate> GetLast();
        Task UpdateAsync(DataUpdate dataFile);
    }

    public class DataUpdateRepository : IDataUpdateRepository
    {
        ILogger<DataUpdateRepository> _logger;
        private readonly CovidContext _context;


        public DataUpdateRepository(CovidContext context,
                                ILogger<DataUpdateRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(DataUpdate dUpdate)
        {
            _context.DataUpdates.Add(dUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DataUpdate>> GetAllAsync()
        {
            return await _context.DataUpdates.ToListAsync();
        }

        public async Task<DataUpdate> GetLast()
        {
            return await _context.DataUpdates.LastAsync();
        }

        public async Task UpdateAsync(DataUpdate dataFile)
        {
            _context.DataUpdates.Update(dataFile);
            await _context.SaveChangesAsync();
        }

    }
}
