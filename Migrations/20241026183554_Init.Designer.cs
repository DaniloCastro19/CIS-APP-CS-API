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
    [Migration("20241026183554_Init")]
    partial class Init
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

                    b.Property<string>("OwnerLogin")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<string>("TopicName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TopicsId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

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

                    b.Property<string>("OwnerLogin")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("topics", (string)null);
                });

            modelBuilder.Entity("cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models.Vote", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("IdeaName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("IdeasId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsPositive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("OwnerLogin")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("votes", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}