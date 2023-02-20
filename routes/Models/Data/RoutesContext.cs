using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Routes.Models.Entities;

namespace Routes.Models.Data
{
    public partial class RoutesContext : DbContext
    {
        public RoutesContext()
        {
        }

        public RoutesContext(DbContextOptions<RoutesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RouteModel> Routes { get; set; } = null!;
        public virtual DbSet<RouteModel> Routes1 { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=HENRIQUE;Database=Routes;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RouteModel>(entity =>
            {
                entity.ToTable("route");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.From)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("from");

                entity.Property(e => e.To)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("to");

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<RouteModel>(entity =>
            {
                entity.ToTable("Routes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.From)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("from");

                entity.Property(e => e.To)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("to");

                entity.Property(e => e.Value).HasColumnName("value");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
