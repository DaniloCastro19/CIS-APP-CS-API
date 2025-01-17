using System;
using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(36).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Login).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Password).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("topics");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(36).IsRequired();
                entity.Property(e => e.Title).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(255).IsRequired();
                entity.Property(e => e.CreationDate).IsRequired();


                // entity.HasMany(i => i.Ideas)
                //     .HasForeignKey(i => i.Idea)
                //     .OnDelete(DeleteBehavior.Cascade);

            });
            
            modelBuilder.Entity<Idea>(entity =>
            {
                entity.ToTable("ideas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(36).IsRequired();
                entity.Property(e => e.Content).HasMaxLength(2000);
                entity.Property(e => e.CreationDate).IsRequired();

                // entity.HasOne(i => i.User) ¿Cómo hago esta relación?
                //     .WithMany(u => u.Ideas) 
                //     .HasForeignKey(i => i.UsersId)
                //     .OnDelete(DeleteBehavior.Cascade);

                // entity.HasOne(i => i.Topic)
                //     .WithMany(t => t.Ideas) 
                //     .HasForeignKey(i => i.TopicsId)
                //     .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Vote>(entity =>
            {
                entity.ToTable("votes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(36).IsRequired();
                entity.Property(e => e.IsPositive).IsRequired();

                // entity.HasOne(v => v.User)
                //     .WithMany(u => u.Votes)
                //     .HasForeignKey(v => v.UsersId)
                //     .OnDelete(DeleteBehavior.Cascade);

                // entity.HasOne(v => v.Idea)
                //     .WithMany(i => i.Votes)
                //     .HasForeignKey(v => v.IdeasId)
                //     .OnDelete(DeleteBehavior.Cascade);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}