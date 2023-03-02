﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyPet.Models;

#nullable disable

namespace MyPet.Migrations
{
    [DbContext(typeof(ProductDbContext))]
    partial class ProductDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MyPet.Models.ExtraImageModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileSource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductModelId");

                    b.ToTable("ExtraImages");
                });

            modelBuilder.Entity("MyPet.Models.MainProductModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Appointment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("BatteryСapacity")
                        .HasColumnType("float");

                    b.Property<double?>("BluetoothVersion")
                        .HasColumnType("float");

                    b.Property<double?>("ChargingTime")
                        .HasColumnType("float");

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConnectionType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConstructionType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<double?>("DefaultPrice")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastTimeEdited")
                        .HasColumnType("datetime2");

                    b.Property<string>("MainFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MarketLaunchDate")
                        .HasColumnType("int");

                    b.Property<double?>("MaxPrice")
                        .HasColumnType("float");

                    b.Property<double?>("MaxRunTime")
                        .HasColumnType("float");

                    b.Property<double?>("MaxRunTimeWithCase")
                        .HasColumnType("float");

                    b.Property<double?>("MinPrice")
                        .HasColumnType("float");

                    b.Property<string>("ParsedUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SummaryStroke")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MyPet.Models.ExtraImageModel", b =>
                {
                    b.HasOne("MyPet.Models.MainProductModel", "ProductModel")
                        .WithMany("ExtraImage")
                        .HasForeignKey("ProductModelId");

                    b.Navigation("ProductModel");
                });

            modelBuilder.Entity("MyPet.Models.MainProductModel", b =>
                {
                    b.Navigation("ExtraImage");
                });
#pragma warning restore 612, 618
        }
    }
}
