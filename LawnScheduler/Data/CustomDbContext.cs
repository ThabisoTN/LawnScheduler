using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LawnScheduler.Data;

public partial class CustomDbContext : DbContext
{
    public CustomDbContext()
    {
    }

    public CustomDbContext(DbContextOptions<CustomDbContext> options)
        : base(options)
    {
    }
   
    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Machine> Machines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__73951AED755C7B60");

            entity.HasIndex(e => e.MachineId, "IX_Bookings_MachineId");

            entity.Property(e => e.BookingDate).HasColumnType("datetime");
            entity.Property(e => e.CompletionDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasMaxLength(450);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ScheduledDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("In Progress");

            entity.HasOne(d => d.Machine).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.MachineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_Machine");
        });

        modelBuilder.Entity<Machine>(entity =>
        {
            entity.HasIndex(e => e.OperatorId, "IX_Machines_OperatorId");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.MachineName).HasMaxLength(100);
            entity.Property(e => e.Model).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
