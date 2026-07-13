using System;
using System.Collections.Generic;
using System.Text;
using IEC104.Master.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IEC104.Master.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Point> Points { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<PeriodicRequest> PeriodicRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // تنظیمات جدول Points
            modelBuilder.Entity<Point>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.InformationObjectAddress)
                    .IsUnique(); // IOA باید یکتا باشد
                entity.Property(e => e.InformationObjectAddress)
                    .IsRequired();
                entity.Property(e => e.Name)
                    .HasMaxLength(255);
                entity.Property(e => e.TypeName)
                    .HasMaxLength(100);
            });

            // تنظیمات جدول Events
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Value)
                    .IsRequired();
                entity.Property(e => e.Quality)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.Timestamp)
                    .IsRequired();

                // رابطه‌ی Many-to-One
                entity.HasOne(e => e.Point)
                    .WithMany(p => p.Events)
                    .HasForeignKey(e => e.PointId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<PeriodicRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Parameter)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.Type)
                    .IsRequired();
                entity.Property(e => e.IntervalMinutes)
                    .IsRequired();
                entity.HasIndex(e => e.IsActive);
            });
        }
    }
}