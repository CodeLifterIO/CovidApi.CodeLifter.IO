using System;
using System.Diagnostics.CodeAnalysis;
using CodeLifter.Covid19.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeLifter.Covid19.Data
{
    public class CovidContext : DbContext
    {
        public static string SQL_CONNECTION_STRING
        {
            get
            {
                string dataSource = Environment.GetEnvironmentVariable("SQLSERVER_DATASOURCE");
                string catalog = Environment.GetEnvironmentVariable("SQLSERVER_CATALOG");
                string userId = Environment.GetEnvironmentVariable("SQLSERVER_USER_ID");
                string password = Environment.GetEnvironmentVariable("SQLSERVER_USER_PASSWORD");

                string connection = $"Data Source={dataSource};Initial Catalog={catalog};trusted_connection=False;User Id={userId};Password={password}";
                return connection;
            }
                
        }
        public DbSet<DataPoint> DataPoints { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<DataCollectionStatistic> DataCollectionStatistics {get; set;}
        public DbSet<GeoCoordinate> GeoCoordinates { get; set; }
        public DbSet<StoredProcedure> StoredProcedures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(SQL_CONNECTION_STRING);
        }
            
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>()
                    .HasIndex(c => c.Name)
                    .IsUnique();

            builder.Entity<District>()
                    .HasIndex(d => new{ d.FIPS, d.Name, })
                    .IsUnique();

            builder.Entity<GeoCoordinate>()
                    .HasIndex(d => new { d.Latitude, d.Longitude, })
                    .IsUnique();

            builder.Entity<Province>()
                    .HasIndex(p => p.Name)
                    .IsUnique();

            builder.Entity<DataPoint>()
                    .HasIndex(d => new {d.LastUpdate, d.CountryId, d.ProvinceId, d.DistrictId })
                    .IsUnique()
                    .HasFilter("[LastUpdate] IS NOT NULL AND [CountryId] IS NOT NULL");
                    
            builder.Entity<DataCollectionStatistic>()
                    .HasIndex(dcs => dcs.Id)
                    .IsUnique();
        }
    }
}