﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using cis_api_legacy_integration_phase_2.Src.Data.Context;

#nullable disable

namespace cis_api_legacy_integration_phase_2.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240930214716_CreateIdeasTable")]
    partial class CreateIdeasTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("cis_api_legacy_integration_phase_2.Core.Abstractions.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models.Idea", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Content")
                        .HasMaxLength(2000)
                        .HasColumnType("varchar(2000)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("TopicsId")
                        .IsRequired()
                        .HasColumnType("varchar(36)");

                    b.Property<string>("UsersId")
                        .IsRequired()
                        .HasColumnType("varchar(36)");

                    b.HasKey("Id");

                    b.HasIndex("TopicsId");

                    b.HasIndex("UsersId");

                    b.ToTable("ideas", (string)null);
                });

            modelBuilder.Entity("cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models.Topic", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UsersId")
                        .IsRequired()
                        .HasColumnType("varchar(36)");

                    b.HasKey("Id");

                    b.HasIndex("UsersId");

                    b.ToTable("topics", (string)null);
                });

            modelBuilder.Entity("cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models.Idea", b =>
                {
                    b.HasOne("cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models.Topic", "Topic")
                        .WithMany("Ideas")
                        .HasForeignKey("TopicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("cis_api_legacy_integration_phase_2.Core.Abstractions.Models.User", "User")
                        .WithMany("Ideas")
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Topic");

                    b.Navigation("User");
                });

            modelBuilder.Entity("cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models.Topic", b =>
                {
                    b.HasOne("cis_api_legacy_integration_phase_2.Core.Abstractions.Models.User", "User")
                        .WithMany("Topics")
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("cis_api_legacy_integration_phase_2.Core.Abstractions.Models.User", b =>
                {
                    b.Navigation("Ideas");

                    b.Navigation("Topics");
                });

            modelBuilder.Entity("cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models.Topic", b =>
                {
                    b.Navigation("Ideas");
                });
#pragma warning restore 612, 618
        }
    }
}
