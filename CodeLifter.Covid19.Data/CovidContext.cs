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
                string baseConnection = "Data Source=10.200.200.100,1401;Initial Catalog=Covid19;trusted_connection=False;User Id=sa;Password=";
                string password = Environment.GetEnvironmentVariable("SQLSERVER_SA_PASSWORD");
                //string password = "Str0ngP@ssw0rd";
                return baseConnection + password;
            }
                
        }

        public DbSet<AdminToken> AdminTokens { get; set; }
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
            builder.Entity<AdminToken>()
                    .HasIndex(c => c.Token)
                    .IsUnique();

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