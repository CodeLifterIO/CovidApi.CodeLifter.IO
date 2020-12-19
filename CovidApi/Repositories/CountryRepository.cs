using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidApi.Data;
using CovidApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CovidApi.Repositories
{
    public interface ICountryRepository
    {
        Task<Country> FindAsync(string slug);
        Task UpsertAsync(Country newCountry);
        Task UpsertRangeAsync(List<Country> countries);
    }

    public class CountryRepository : ICountryRepository
    {
        private readonly CovidContext _context;

        public CountryRepository(CovidContext context)
        {
            _context = context;
        }

        public async Task<Country> FindAsync(string slug)
        {
            return await _context.Countries
                .Where(c => c.SlugId == slug)
                .FirstOrDefaultAsync();
        }

        public async Task UpsertAsync(Country newCountry)
        {
            await _context.Countries.Upsert(newCountry)
                                .On(e => e.SlugId)
                                .WhenMatched((eDB, eIn) => new Country
                                {
                                    Name = newCountry.Name,
                                    UpdatedAt = DateTime.UtcNow,
                                    GeoCoordinateId = newCountry.GeoCoordinateId,
                                })
                                .RunAsync();
        }

        public async Task UpsertRangeAsync(List<Country> countries)
        {
            await _context.Countries.UpsertRange(countries)
                    .On(e => e.SlugId)
                    .WhenMatched((eDB, eIn) => new Country
                    {
                        Name = eIn.Name,
                        UpdatedAt = DateTime.UtcNow,
                        GeoCoordinateId = eIn.GeoCoordinateId,
                    })
                    .RunAsync();
        }
    }
}
