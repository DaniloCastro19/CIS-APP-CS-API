using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public partial class Sd3Context : DbContext
{
    public Sd3Context()
    {
    }

    public Sd3Context(DbContextOptions<Sd3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Idea> Ideas { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vote> Votes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=127.0.0.1;port=3307;database=sd3;uid=root;password=sd5", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.3.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Idea>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ideas");

            entity.HasIndex(e => e.TopicsId, "topics_id");

            entity.HasIndex(e => e.UsersId, "users_id");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .HasDefaultValueSql("uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Content)
                .HasMaxLength(2000)
                .HasColumnName("content");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.TopicsId)
                .HasMaxLength(36)
                .HasColumnName("topics_id");
            entity.Property(e => e.UsersId).HasColumnName("users_id");

            entity.HasOne(d => d.Topics).WithMany(p => p.Ideas)
                .HasForeignKey(d => d.TopicsId)
                .HasConstraintName("ideas_ibfk_2");

            entity.HasOne(d => d.Users).WithMany(p => p.Ideas)
                .HasForeignKey(d => d.UsersId)
                .HasConstraintName("ideas_ibfk_1");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("topics");

            entity.HasIndex(e => e.UsersId, "users_id");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .HasDefaultValueSql("uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
            entity.Property(e => e.UsersId).HasColumnName("users_id");

            entity.HasOne(d => d.Users).WithMany(p => p.Topics)
                .HasForeignKey(d => d.UsersId)
                .HasConstraintName("topics_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "UKow0gan20590jrb00upg3va2fn").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Login)
                .HasMaxLength(16)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Vote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("votes");

            entity.HasIndex(e => e.IdeasId, "ideas_id");

            entity.HasIndex(e => e.UsersId, "users_id");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .HasDefaultValueSql("uuid()")
                .HasColumnName("id");
            entity.Property(e => e.IdeasId)
                .HasMaxLength(36)
                .HasColumnName("ideas_id");
            entity.Property(e => e.IsPositive).HasColumnName("is_positive");
            entity.Property(e => e.UsersId).HasColumnName("users_id");

            entity.HasOne(d => d.Ideas).WithMany(p => p.Votes)
                .HasForeignKey(d => d.IdeasId)
                .HasConstraintName("votes_ibfk_2");

            entity.HasOne(d => d.Users).WithMany(p => p.Votes)
                .HasForeignKey(d => d.UsersId)
                .HasConstraintName("votes_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
