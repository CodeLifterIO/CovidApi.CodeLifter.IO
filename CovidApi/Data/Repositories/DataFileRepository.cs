using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CovidApi.Data.Repositories
{
    public interface IDataFileRepository
    {
        Task<List<DataFile>> GetAllAsync();
        Task<DataFile> FindAsync(int id);
        Task UpdateAsync(DataFile dataFile);
        Task DeleteAsync(int id);
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
            return await _context.DataFiles.ToListAsync();
        }

        public async Task<DataFile> FindAsync(int id)
        {
            return await _context.DataFiles
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(DataFile dataFile)
        {
            _context.Update(dataFile);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task DeleteAsync(int id)
        {
            var DataFile = await FindAsync(id);
            _context.DataFiles.Remove(DataFile);
            await _context.SaveChangesAsync();
        }
    }
}
