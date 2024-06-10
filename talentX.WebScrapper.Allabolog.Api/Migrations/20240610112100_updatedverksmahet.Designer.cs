﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using talentX.WebScrapper.Allabolog.Repositories.Data;

#nullable disable

namespace talentX.WebScrapper.Allabolog.Api.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240610112100_updatedverksmahet")]
    partial class updatedverksmahet
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.30")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("talentX.WebScrapper.Allabolog.Entities.DetailedScrapOutputData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AllabolagUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CEO")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfSearch")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmployeeNames")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrgNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Revenue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SearchFieldText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Verksamhet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("YearOfEstablishment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DetailedScrapOutputData");
                });

            modelBuilder.Entity("talentX.WebScrapper.Allabolog.Entities.InitialScrapOutputData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Verksamhet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InitialScrapOutputData");
                });
#pragma warning restore 612, 618
        }
    }
}
