using System;
using System.Diagnostics;
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
                string SQLSERVER_CONNECTION_STRING = "Data Source=10.200.200.101;Initial Catalog=Covid19;trusted_connection=False;User Id=sa;Password=Str0ngP@ssw0rd";
                //string SQLSERVER_CONNECTION_STRING = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");
                if (string.IsNullOrEmpty(SQLSERVER_CONNECTION_STRING))
                {
                    string dataSource = Environment.GetEnvironmentVariable("SQLSERVER_DATASOURCE");
                    string catalog = Environment.GetEnvironmentVariable("SQLSERVER_CATALOG");
                    string userId = Environment.GetEnvironmentVariable("SQLSERVER_USER_ID");
                    string password = Environment.GetEnvironmentVariable("SQLSERVER_USER_PASSWORD");
                    SQLSERVER_CONNECTION_STRING = $"Data Source={dataSource};Initial Catalog={catalog};trusted_connection=False;User Id={userId};Password={password}";
                }
                return SQLSERVER_CONNECTION_STRING;
            }
        }

        public DbSet<DataPoint> DataPoints { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<GeoCoordinate> GeoCoordinates { get; set; }
        public DbSet<Totals> Totals { get; set; }
        public DbSet<DataCollectionStatistic> DataCollectionStatistics {get; set;}
        public DbSet<StoredProcedure> StoredProcedures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(SQL_CONNECTION_STRING);
        }
            
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DataPoint>()
                    .HasIndex(d => new { d.LastUpdate, d.CountryId, d.ProvinceId, d.DistrictId })
                    .IsUnique()
                    .HasFilter("[LastUpdate] IS NOT NULL AND [CountryId] IS NOT NULL");

            builder.Entity<Country>()
                    .HasIndex(c => c.Name)
                    .IsUnique();

            builder.Entity<District>()
                    .HasIndex(d => new{ d.FIPS, d.Name, })
                    .IsUnique();

            builder.Entity<Province>()
                    .HasIndex(p => p.Name)
                    .IsUnique();

            builder.Entity<GeoCoordinate>()
                    .HasIndex(d => new { d.Latitude, d.Longitude, })
                    .IsUnique();

            builder.Entity<Totals>()
                    .HasIndex(d => new { d.SourceFile, d.CountryId, d.ProvinceId, d.DistrictId, })
                    .IsUnique();

            builder.Entity<DataCollectionStatistic>()
                    .HasIndex(dcs => dcs.Id)
                    .IsUnique();
        }
    }
}