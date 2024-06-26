﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api.Repositories;

#nullable disable

namespace api.Repositories.Migrations
{
    [DbContext(typeof(TestTaskDbContext))]
    [Migration("20240409114730_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("api.Common.EfBuilderExtensions+EnumTable<api.DomainModels.Gender>", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.ToTable("Gender", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "Male"
                        },
                        new
                        {
                            Id = "Female"
                        },
                        new
                        {
                            Id = "Other"
                        },
                        new
                        {
                            Id = "Unknown"
                        });
                });

            modelBuilder.Entity("api.DomainModels.Patient", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameId")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("Gender");

                    b.HasIndex("NameId")
                        .IsUnique();

                    b.ToTable("Patient", (string)null);
                });

            modelBuilder.Entity("api.DomainModels.PatientGiven", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Given")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatientNameId")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("PatientNameId");

                    b.ToTable("PatientGiven");
                });

            modelBuilder.Entity("api.DomainModels.PatientName", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Family")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Use")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PatientName");
                });

            modelBuilder.Entity("api.DomainModels.Patient", b =>
                {
                    b.HasOne("api.Common.EfBuilderExtensions+EnumTable<api.DomainModels.Gender>", null)
                        .WithMany()
                        .HasForeignKey("Gender")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("api.DomainModels.PatientName", "Name")
                        .WithOne()
                        .HasForeignKey("api.DomainModels.Patient", "NameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Name");
                });

            modelBuilder.Entity("api.DomainModels.PatientGiven", b =>
                {
                    b.HasOne("api.DomainModels.PatientName", null)
                        .WithMany("Given")
                        .HasForeignKey("PatientNameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("api.DomainModels.PatientName", b =>
                {
                    b.Navigation("Given");
                });
#pragma warning restore 612, 618
        }
    }
}
