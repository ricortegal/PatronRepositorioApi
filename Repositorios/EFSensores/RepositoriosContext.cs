using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repositorios.EFSensores;

public partial class RepositoriosContext : DbContext
{
    public RepositoriosContext()
    {
    }

    public RepositoriosContext(DbContextOptions<RepositoriosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SensorEf> Sensores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;uid=root;pwd=alberto01;database=repositorios", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<SensorEf>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sensores");

            entity.Property(e => e.Id).HasMaxLength(45);
            entity.Property(e => e.Nombre).HasMaxLength(45);
            entity.Property(e => e.UnidadMedida).HasMaxLength(45);
           
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
