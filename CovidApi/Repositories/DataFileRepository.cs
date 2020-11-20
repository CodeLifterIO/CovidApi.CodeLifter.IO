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
    public interface IDataFileRepository
    {
        Task<List<DataFile>> GetAllAsync();
        Task<DataFile> GetLast();
        Task<DataFile> FindAsync(int id);
        Task<DataFile> FindAsync(string fileName);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsAsync(string fileName);
        Task AddAsync(DataFile dataFile);
        Task UpdateAsync(DataFile dataFile);
    }

    public class DataFileRepository : IDataFileRepository
    {
        ILogger<DataFileRepository> _logger;
        private readonly CovidContext _context;
        public DataFileRepository(CovidContext context,
                                ILogger<DataFileRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<DataFile>> GetAllAsync()
        {
            return await _context.DataFiles.OrderByDescending(x => x.FileName).ToListAsync();
        }

        public async Task<DataFile> FindAsync(int id)
        {
            if (id < 1) return null;
            return await _context.DataFiles
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<DataFile> FindAsync(string fileName)
        {
            return await _context.DataFiles
                .FirstOrDefaultAsync(c => c.FileName == fileName);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DataFiles.AnyAsync(c => c.Id == id);
        }
        public async Task<bool> ExistsAsync(string fileName)
        {
            return await _context.DataFiles.AnyAsync(c => c.FileName == fileName);
        }

        public async Task AddAsync(DataFile dataFile)
        {
            var existing = await FindAsync(dataFile.FileName);
            if (null != existing)
            {
                return;
            }
            _context.DataFiles.Add(dataFile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DataFile dataFile)
        {
            _context.DataFiles.Update(dataFile);
            await _context.SaveChangesAsync();
        }

        public async Task<DataFile> GetLast()
        {
            return await _context.DataFiles.LastAsync();
        }
    }
}
