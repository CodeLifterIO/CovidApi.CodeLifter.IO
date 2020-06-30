using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using CodeLifter.Covid19.Data.Models;
using CodeLifter.Covid19.Data.Models.BaseEntities;
using Microsoft.EntityFrameworkCore;

namespace CodeLifter.Covid19.Data
{
    public class CovidContext : DbContext
    {
        public static string SQL_CONNECTION_STRING
        {
            get
            {
                //string SQLSERVER_CONNECTION_STRING = "Data Source=10.200.200.100;Initial Catalog=Covid19;trusted_connection=False;User Id=sa;Password=S@berh@gen01SqlServer";
                string SQLSERVER_CONNECTION_STRING = "Data Source=10.200.200.100;Initial Catalog=DevCovid19;trusted_connection=False;User Id=sa;Password=S@berh@gen01SqlServer";
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
        public DbSet<Total> Totals { get; set; }
        public DbSet<DataFile> DataFiles {get; set;}
        public DbSet<DataUpdate> DataUpdates { get; set; }
        public DbSet<StoredProcedure> StoredProcedures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(SQL_CONNECTION_STRING);
        }
            
        protected override void OnModelCreating(ModelBuilder builder)
        {
            OnDataPointModelCreating(builder);
            OnCountrytModelCreating(builder);
            OnProvinceModelCreating(builder);
            OnDistrictModelCreating(builder);
            OnGeoCoordinateModelCreating(builder);
            OnTotalModelCreating(builder);
            OnDataFileModelCreating(builder);
            OnDataUpdateModelCreating(builder);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Entity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((Entity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((Entity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }

        //DataPoint
        protected void OnDataPointModelCreating(ModelBuilder builder)
        {
            builder.Entity<DataPoint>()
                .HasIndex(d => new { d.SourceFile, d.CountrySlugId, d.ProvinceSlugId, d.DistrictSlugId })
                .IsUnique()
                .HasFilter("[SourceFile] IS NOT NULL AND [CountrySlugId] IS NOT NULL");
        }

        //Country
        protected void OnCountrytModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>()
                .HasAlternateKey(c => c.SlugId);
        }

        //Province
        protected void OnProvinceModelCreating(ModelBuilder builder)
        {
            builder.Entity<Province>()
                .HasAlternateKey(p => p.SlugId);
        }

        //District
        protected void OnDistrictModelCreating(ModelBuilder builder)
        {
            builder.Entity<District>()
                .HasAlternateKey(d => new { d.SlugId });
        }


        //GeoCoordinate
        protected void OnGeoCoordinateModelCreating(ModelBuilder builder)
        {
            builder.Entity<GeoCoordinate>()
                .HasIndex(d => new { d.Id })
                .IsUnique();
        }

        //Total
        protected void OnTotalModelCreating(ModelBuilder builder)
        {
            builder.Entity<Total>()
                .HasIndex(d => new { d.SourceFile, d.CountrySlugId, d.ProvinceSlugId, d.DistrictSlugId, })
                .IsUnique();
        }

        //DataFile
        protected void OnDataFileModelCreating(ModelBuilder builder)
        {
            builder.Entity<DataFile>()
                .HasKey(df => df.Id);

            //builder.Entity<DataFile>().Property(e.Id).ValueGeneratedOnAdd();
        }

        protected void OnDataUpdateModelCreating(ModelBuilder builder)
        {
            builder.Entity<DataUpdate>()
                .HasIndex(du => du.Id)
                .IsUnique();
        }
    }
}