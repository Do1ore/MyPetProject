﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyPet.Models;

#nullable disable

namespace MyPet.Migrations
{
    [DbContext(typeof(ProductDbContext))]
    [Migration("20230228143050_init5")]
    partial class init5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<int?>("headphoneModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("headphoneModelId");

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

                    b.Property<DateTime>("LastTimeEdited")
                        .HasColumnType("datetime2");

                    b.Property<string>("MainFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MarketLaunchDate")
                        .HasColumnType("int");

                    b.Property<double?>("MaxRunTime")
                        .HasColumnType("float");

                    b.Property<double?>("MaxRunTimeWithCase")
                        .HasColumnType("float");

                    b.Property<string>("ParsedUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ProductType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SummaryStroke")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Headphones");
                });

            modelBuilder.Entity("MyPet.Models.ExtraImageModel", b =>
                {
                    b.HasOne("MyPet.Models.MainProductModel", "headphoneModel")
                        .WithMany("ExtraImages")
                        .HasForeignKey("headphoneModelId");

                    b.Navigation("headphoneModel");
                });

            modelBuilder.Entity("MyPet.Models.MainProductModel", b =>
                {
                    b.Navigation("ExtraImages");
                });
#pragma warning restore 612, 618
        }
    }
}