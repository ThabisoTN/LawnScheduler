﻿// <auto-generated />
using System;
using LawnScheduler.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LawnScheduler.Migrations.CustomDb
{
    [DbContext(typeof(CustomDbContext))]
    [Migration("20241012134258_BookingTbale")]
    partial class BookingTbale
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LawnScheduler.Data.Booking", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingId"));

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsConflict")
                        .HasColumnType("bit");

                    b.Property<int>("MachineId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ScheduledDate")
                        .HasColumnType("datetime");

                    b.HasKey("BookingId")
                        .HasName("PK__Bookings__73951AED755C7B60");

                    b.HasIndex("MachineId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("LawnScheduler.Data.Machine", b =>
                {
                    b.Property<int>("MachineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MachineId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("MachineName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("OperatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MachineId");

                    b.HasIndex(new[] { "OperatorId" }, "IX_Machines_OperatorId");

                    b.ToTable("Machines");
                });

            modelBuilder.Entity("LawnScheduler.Data.Booking", b =>
                {
                    b.HasOne("LawnScheduler.Data.Machine", "Machine")
                        .WithMany("Bookings")
                        .HasForeignKey("MachineId")
                        .IsRequired()
                        .HasConstraintName("FK_Booking_Machine");

                    b.Navigation("Machine");
                });

            modelBuilder.Entity("LawnScheduler.Data.Machine", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
