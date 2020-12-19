using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidApi.Data;
using Microsoft.EntityFrameworkCore;

namespace CovidApi.Repositories
{
    public interface ITotalRepository
    {
        Task SummarizeRegions(string sourceFile);
    }

    public class TotalRepository : ITotalRepository
    {
        private readonly CovidContext _context;

        public TotalRepository(CovidContext context)
        {
            _context = context;
        }

        public async Task SummarizeRegions(string sourceFile)
        {
            sourceFile = sourceFile.Replace(".csv", "");

            await SummarizeCountriesAsync(sourceFile);
            await SummarizeProvincesAsync(sourceFile);
            await SummarizeDistrictsAsync(sourceFile);
        }

        private async Task SummarizeCountriesAsync(string sourceFile)
        {
            string raw = $@"SET NOCOUNT ON;
							INSERT INTO Totals (CountryId, Count, Confirmed, Deaths, Recovered, Active, SourceFile)
							SELECT	
								dp.CountryId AS CountryId,
								COUNT(*) AS Count,
								SUM(dp.Confirmed) AS Confirmed,
                                SUM(dp.Deaths) AS Deaths,
                                SUM(dp.Recovered) AS Recovered,
                                SUM(dp.Active) AS Active,
								dp.SourceFile AS SourceFile
                            FROM dbo.DataPoints dp
                            WHERE dp.SourceFile = ""{sourceFile}""
                            GROUP BY dp.CountryId, dp.SourceFile";

            await _context.Database.ExecuteSqlRawAsync(raw);
        }

        private async Task SummarizeProvincesAsync(string sourceFile)
        {
            string raw = $@"SET NOCOUNT ON;
							INSERT INTO Totals (ProvinceId, Count, Confirmed, Deaths, Recovered, Active, SourceFile)
							SELECT	
								dp.ProvinceId AS CountryId,
								COUNT(*) AS Count,
								SUM(dp.Confirmed) AS Confirmed,
                                SUM(dp.Deaths) AS Deaths,
                                SUM(dp.Recovered) AS Recovered,
                                SUM(dp.Active) AS Active,
								dp.SourceFile AS SourceFile
                            FROM dbo.DataPoints dp
                            WHERE dp.SourceFile = ""{sourceFile}""
                            GROUP BY dp.ProvinceId, dp.SourceFile";

            await _context.Database.ExecuteSqlRawAsync(raw);
        }

        private async Task SummarizeDistrictsAsync(string sourceFile)
        {
            string raw = $@"SET NOCOUNT ON;
							INSERT INTO Totals (DistrictId, Count, Confirmed, Deaths, Recovered, Active, SourceFile)
							SELECT	
								dp.DistrictId AS CountryId,
								COUNT(*) AS Count,
								SUM(dp.Confirmed) AS Confirmed,
                                SUM(dp.Deaths) AS Deaths,
                                SUM(dp.Recovered) AS Recovered,
                                SUM(dp.Active) AS Active,
								dp.SourceFile AS SourceFile
                            FROM dbo.DataPoints dp
                            WHERE dp.SourceFile = ""{sourceFile}""
                            GROUP BY dp.DistrictId, dp.SourceFile";

            await _context.Database.ExecuteSqlRawAsync(raw);
        }
    }
}
