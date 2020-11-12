using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using CovidApi.Models;
using CovidApi.Models.BaseEntities;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CovidApi.Data
{
    public partial class CovidContext : IdentityDbContext<ApplicationUser>, IDataProtectionKeyContext
    {
        public static string SQL_CONNECTION_STRING
        {
            get
            {
                //string SQLSERVER_CONNECTION_STRING = "Data Source=10.200.200.100;Initial Catalog=Covid19;trusted_connection=False;User Id=sa;Password=S@berh@gen01SqlServer";
                //string SQLSERVER_CONNECTION_STRING = "Data Source=10.200.200.100;Initial Catalog=DevCovid19;trusted_connection=False;User Id=sa;Password=S@berh@gen01SqlServer";
                string SQLSERVER_CONNECTION_STRING = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");
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

        public string CurrentUserId { get; set; } = "";

        //AUTH
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;

        //Self Contained Entities
        //public DbSet<DataPoint> DataPoints { get; set; }
        //public DbSet<Country> Countries { get; set; }
        //public DbSet<District> Districts { get; set; }
        //public DbSet<Province> Provinces { get; set; }
        //public DbSet<GeoCoordinate> GeoCoordinates { get; set; }
        //public DbSet<Total> Totals { get; set; }
        //public DbSet<DataFile> DataFiles {get; set;}
        public DbSet<DataUpdate> DataUpdates { get; set; }
        //public DbSet<StoredProcedure> StoredProcedures { get; set; }

        public CovidContext(DbContextOptions<CovidContext> options): base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(SQL_CONNECTION_STRING);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "ApplicationUsers");
            });

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
                .HasKey(dp => dp.Id);
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
