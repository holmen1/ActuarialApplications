﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ActuarialApplications.Migrations
{
    [DbContext(typeof(LocalDbContext))]
    [Migration("20230514064634_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("ActuarialApplications.Models.Swap", b =>
                {
                    b.Property<DateTime>("ValueDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Currency")
                        .HasColumnType("TEXT");

                    b.Property<int>("Tenor")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("SettlementFreq")
                        .HasColumnType("REAL");

                    b.Property<double?>("Value")
                        .HasColumnType("REAL");

                    b.HasKey("ValueDate", "Currency", "Tenor");

                    b.ToTable("Swap");
                });
#pragma warning restore 612, 618
        }
    }
}