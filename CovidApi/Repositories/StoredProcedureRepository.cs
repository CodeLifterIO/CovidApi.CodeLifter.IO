using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidApi.Data;
using Microsoft.EntityFrameworkCore;

namespace CovidApi.Repositories
{
    public interface IStoredProcedureRepository
    {
        Task SummarizeEntities();
    }

    public class StoredProcedureRepository : IStoredProcedureRepository
    {
        private readonly CovidContext _context;

        public StoredProcedureRepository(CovidContext context)
        {
            _context = context;
        }

        public async Task SummarizeEntities()
        {
            await SummarizeCountries();
            await SummarizeProvinces();
            await SummarizeDistricts();
        }

        private async Task SummarizeCountries()
        {
            await _context.Database.ExecuteSqlRawAsync($"EXEC SP_Update_Summary_On_Country;");
        }

        private async Task SummarizeProvinces()
        {
            await _context.Database.ExecuteSqlRawAsync($"EXEC SP_Update_Summary_On_Province;");
        }

        private async Task SummarizeDistricts()
        {
            await _context.Database.ExecuteSqlRawAsync($"EXEC SP_Update_Summary_On_District;");
        }
    }
}
