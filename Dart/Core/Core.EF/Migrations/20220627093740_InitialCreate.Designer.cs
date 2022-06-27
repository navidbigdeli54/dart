﻿// <auto-generated />
using System;
using Core.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.EF.Migrations
{
    [DbContext(typeof(EFContext))]
    [Migration("20220627093740_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Core.Domain.Model.GameSession", b =>
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

                    b.ToTable("tblGameSession", (string)null);
                });

            modelBuilder.Entity("Core.Domain.Model.Leaderboard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("GameSessionId")
                        .HasColumnType("uuid");

                    b.Property<int>("Rank")
                        .HasColumnType("integer");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameSessionId")
                        .IsUnique();

                    b.ToTable("tblLeaderboard", (string)null);
                });

            modelBuilder.Entity("Core.Domain.Model.Score", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("GameSessionId")
                        .HasColumnType("uuid");

                    b.Property<int>("Point")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameSessionId");

                    b.ToTable("tblScore", (string)null);
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

                    b.ToTable("tblUser", (string)null);
                });

            modelBuilder.Entity("Core.Domain.Model.GameSession", b =>
                {
                    b.HasOne("Core.Domain.Model.User", null)
                        .WithOne()
                        .HasForeignKey("Core.Domain.Model.GameSession", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.Model.Leaderboard", b =>
                {
                    b.HasOne("Core.Domain.Model.GameSession", null)
                        .WithOne()
                        .HasForeignKey("Core.Domain.Model.Leaderboard", "GameSessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.Model.Score", b =>
                {
                    b.HasOne("Core.Domain.Model.GameSession", null)
                        .WithMany()
                        .HasForeignKey("GameSessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}