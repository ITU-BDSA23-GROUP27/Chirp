﻿// <auto-generated />
using System;
using CheepRepository;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CheepRepository.Migrations
{
    [DbContext(typeof(ChirpDbContext))]
    [Migration("20231012115245_IdReplacedWithGUID")]
    partial class IdReplacedWithGUID
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("Chirp.Chirp.Core.Author", b =>
                {
                    b.Property<Guid>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("AuthorId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("Chirp.Chirp.Core.Cheep", b =>
                {
                    b.Property<Guid>("CheepId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(160)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("TEXT");

                    b.HasKey("CheepId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Cheeps");
                });

            modelBuilder.Entity("Chirp.Chirp.Core.Follower", b =>
                {
                    b.Property<Guid>("FollowerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("FolloweeId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("FolloweeAuthorAuthorId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("FollowerAuthorAuthorId")
                        .HasColumnType("TEXT");

                    b.HasKey("FollowerId", "FolloweeId");

                    b.HasIndex("FolloweeAuthorAuthorId");

                    b.HasIndex("FollowerAuthorAuthorId");

                    b.ToTable("Follower");
                });

            modelBuilder.Entity("Chirp.Chirp.Core.Cheep", b =>
                {
                    b.HasOne("Chirp.Chirp.Core.Author", "Author")
                        .WithMany("Cheeps")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Chirp.Chirp.Core.Follower", b =>
                {
                    b.HasOne("Chirp.Chirp.Core.Author", "FolloweeAuthor")
                        .WithMany()
                        .HasForeignKey("FolloweeAuthorAuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Chirp.Chirp.Core.Author", "FollowerAuthor")
                        .WithMany()
                        .HasForeignKey("FollowerAuthorAuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FolloweeAuthor");

                    b.Navigation("FollowerAuthor");
                });

            modelBuilder.Entity("Chirp.Chirp.Core.Author", b =>
                {
                    b.Navigation("Cheeps");
                });
#pragma warning restore 612, 618
        }
    }
}
