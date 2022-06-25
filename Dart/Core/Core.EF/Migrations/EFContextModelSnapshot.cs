﻿// <auto-generated />
using System;
using Core.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.EF.Migrations
{
    [DbContext(typeof(EFContext))]
    partial class EFContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Core.Domain.Model.GameSeason", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("GameSeasons");
                });

            modelBuilder.Entity("Core.Domain.Model.LeaderboardEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("GameSeasonId")
                        .HasColumnType("uuid");

                    b.Property<int>("Rank")
                        .HasColumnType("integer");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameSeasonId")
                        .IsUnique();

                    b.ToTable("Leaderboard");
                });

            modelBuilder.Entity("Core.Domain.Model.Score", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("GameSeasonId")
                        .HasColumnType("uuid");

                    b.Property<int>("Point")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameSeasonId");

                    b.ToTable("Score");
                });

            modelBuilder.Entity("Core.Domain.Model.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("EndPoint")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Core.Domain.Model.GameSeason", b =>
                {
                    b.HasOne("Core.Domain.Model.User", null)
                        .WithOne()
                        .HasForeignKey("Core.Domain.Model.GameSeason", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.Model.LeaderboardEntry", b =>
                {
                    b.HasOne("Core.Domain.Model.GameSeason", null)
                        .WithOne()
                        .HasForeignKey("Core.Domain.Model.LeaderboardEntry", "GameSeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.Model.Score", b =>
                {
                    b.HasOne("Core.Domain.Model.GameSeason", null)
                        .WithMany()
                        .HasForeignKey("GameSeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
