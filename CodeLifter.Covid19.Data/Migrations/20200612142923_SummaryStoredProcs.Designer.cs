﻿// <auto-generated />
using System;
using CodeLifter.Covid19.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeLifter.Covid19.Data.Migrations
{
    [DbContext(typeof(CovidContext))]
    [Migration("20200612142923_SummaryStoredProcs")]
    partial class SummaryStoredProcs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CodeLifter.Covid19.Data.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GeoCoordinateId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GeoCoordinateId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("CodeLifter.Covid19.Data.Models.DataCollectionStatistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastRunCompleted")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastRunStarted")
                        .HasColumnType("datetime2");

                    b.Property<int>("RecordsProcessed")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("DataCollectionStatistics");
                });

            modelBuilder.Entity("CodeLifter.Covid19.Data.Models.DataPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Active")
                        .HasColumnType("int");

                    b.Property<double?>("CaseFatalityRatio")
                        .HasColumnType("float");

                    b.Property<int?>("Confirmed")
                        .HasColumnType("int");

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<int?>("Deaths")
                        .HasColumnType("int");

                    b.Property<int?>("DistrictId")
                        .HasColumnType("int");

                    b.Property<double?>("IncidenceRate")
                        .HasColumnType("float");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ProvinceId")
                        .HasColumnType("int");

                    b.Property<int?>("Recovered")
                        .HasColumnType("int");

                    b.Property<string>("SourceFile")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("DistrictId");

                    b.HasIndex("ProvinceId");

                    b.HasIndex("LastUpdate", "CountryId", "ProvinceId", "DistrictId")
                        .IsUnique()
                        .HasFilter("[LastUpdate] IS NOT NULL AND [CountryId] IS NOT NULL");

                    b.ToTable("DataPoints");
                });

            modelBuilder.Entity("CodeLifter.Covid19.Data.Models.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("FIPS")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("GeoCoordinateId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("ProvinceId")
                        .HasColumnType("int");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("GeoCoordinateId");

                    b.HasIndex("ProvinceId");

                    b.HasIndex("FIPS", "Name")
                        .IsUnique()
                        .HasFilter("[FIPS] IS NOT NULL AND [Name] IS NOT NULL");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("CodeLifter.Covid19.Data.Models.GeoCoordinate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double?>("Latitude")
                        .HasColumnType("float");

                    b.Property<double?>("Longitude")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("Latitude", "Longitude")
                        .IsUnique()
                        .HasFilter("[Latitude] IS NOT NULL AND [Longitude] IS NOT NULL");

                    b.ToTable("GeoCoordinates");
                });

            modelBuilder.Entity("CodeLifter.Covid19.Data.Models.Province", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<int?>("GeoCoordinateId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("GeoCoordinateId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("CodeLifter.Covid19.Data.Models.Totals", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Active")
                        .HasColumnType("int");

                    b.Property<int?>("Confirmed")
                        .HasColumnType("int");

                    b.Property<int?>("Count")
                        .HasColumnType("int");

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<int?>("Deaths")
                        .HasColumnType("int");

                    b.Property<int?>("DistrictId")
                        .HasColumnType("int");

                    b.Property<int?>("ProvinceId")
                        .HasColumnType("int");

                    b.Property<int?>("Recovered")
                        .HasColumnType("int");

                    b.Property<string>("SourceFile")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("DistrictId");

                    b.HasIndex("ProvinceId");

                    b.HasIndex("SourceFile", "CountryId", "ProvinceId", "DistrictId")
                        .IsUnique()
                        .HasFilter("[SourceFile] IS NOT NULL AND [CountryId] IS NOT NULL AND [ProvinceId] IS NOT NULL AND [DistrictId] IS NOT NULL");

                    b.ToTable("Totals");
                });

            modelBuilder.Entity("CodeLifter.Covid19.Data.Models.Country", b =>
                {
                    b.HasOne("CodeLifter.Covid19.Data.Models.GeoCoordinate", "GeoCoordinate")
                        .WithMany()
                        .HasForeignKey("GeoCoordinateId");
                });

            modelBuilder.Entity("CodeLifter.Covid19.Data.Models.DataPoint", b =>
                {
                    b.HasOne("CodeLifter.Covid19.Data.Models.Country", "Country")
                        .WithMany("DataPoints")
                        .HasForeignKey("CountryId");

                    b.HasOne("CodeLifter.Covid19.Data.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId");

                    b.HasOne("CodeLifter.Covid19.Data.Models.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceId");
                });

            modelBuilder.Entity("CodeLifter.Covid19.Data.Models.District", b =>
                {
                    b.HasOne("CodeLifter.Covid19.Data.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("CodeLifter.Covid19.Data.Models.GeoCoordinate", "GeoCoordinate")
                        .WithMany()
                        .HasForeignKey("GeoCoordinateId");

                    b.HasOne("CodeLifter.Covid19.Data.Models.Province", "Province")
                        .WithMany("Districts")
                        .HasForeignKey("ProvinceId");
                });

            modelBuilder.Entity("CodeLifter.Covid19.Data.Models.Province", b =>
                {
                    b.HasOne("CodeLifter.Covid19.Data.Models.Country", "Country")
                        .WithMany("Provinces")
                        .HasForeignKey("CountryId");

                    b.HasOne("CodeLifter.Covid19.Data.Models.GeoCoordinate", "GeoCoordinate")
                        .WithMany()
                        .HasForeignKey("GeoCoordinateId");
                });

            modelBuilder.Entity("CodeLifter.Covid19.Data.Models.Totals", b =>
                {
                    b.HasOne("CodeLifter.Covid19.Data.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("CodeLifter.Covid19.Data.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId");

                    b.HasOne("CodeLifter.Covid19.Data.Models.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceId");
                });
#pragma warning restore 612, 618
        }
    }
}
