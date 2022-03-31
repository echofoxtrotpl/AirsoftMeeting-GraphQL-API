﻿// <auto-generated />
using System;
using AirsoftMeetingGraphQL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AirsoftMeetingGraphQL.Migrations
{
    [DbContext(typeof(AirsoftDbContext))]
    [Migration("20220117234245_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("AirsoftMeetingGraphQL.Entities.Event", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("CreatorId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("ImageId")
                        .HasColumnType("bigint");

                    b.Property<long>("LocationId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ImageId")
                        .IsUnique();

                    b.HasIndex("LocationId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("AirsoftMeetingGraphQL.Entities.Image", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("CreatorId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Folder")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("AirsoftMeetingGraphQL.Entities.Location", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<string>("LocationName")
                        .HasColumnType("text");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<string>("Region")
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("AirsoftMeetingGraphQL.Entities.Player", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("JwtPlayerId")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<int?>("TeamId")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("AirsoftMeetingGraphQL.Entities.PlayersJoinedEvent", b =>
                {
                    b.Property<long>("EventId")
                        .HasColumnType("bigint");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.HasKey("EventId", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayersJoinedEvents");
                });

            modelBuilder.Entity("AirsoftMeetingGraphQL.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("TeamName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("AirsoftMeetingGraphQL.Entities.Event", b =>
                {
                    b.HasOne("AirsoftMeetingGraphQL.Entities.Player", "Creator")
                        .WithMany("HostedEvents")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirsoftMeetingGraphQL.Entities.Image", "Image")
                        .WithOne("Event")
                        .HasForeignKey("AirsoftMeetingGraphQL.Entities.Event", "ImageId");

                    b.HasOne("AirsoftMeetingGraphQL.Entities.Location", "Location")
                        .WithMany("Events")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Image");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("AirsoftMeetingGraphQL.Entities.Image", b =>
                {
                    b.HasOne("AirsoftMeetingGraphQL.Entities.Player", "Creator")
                        .WithMany("Images")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("AirsoftMeetingGraphQL.Entities.Player", b =>
                {
                    b.HasOne("AirsoftMeetingGraphQL.Entities.Team", "Team")
                        .WithMany("TeamCrew")
                        .HasForeignKey("TeamId");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("AirsoftMeetingGraphQL.Entities.PlayersJoinedEvent", b =>
                {
                    b.HasOne("AirsoftMeetingGraphQL.Entities.Event", "Event")
                        .WithMany("PlayersJoinedEvent")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirsoftMeetingGraphQL.Entities.Player", "Player")
                        .WithMany("JoinedEvents")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("AirsoftMeetingGraphQL.Entities.Event", b =>
                {
                    b.Navigation("PlayersJoinedEvent");
                });

            modelBuilder.Entity("AirsoftMeetingGraphQL.Entities.Image", b =>
                {
                    b.Navigation("Event");
                });

            modelBuilder.Entity("AirsoftMeetingGraphQL.Entities.Location", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("AirsoftMeetingGraphQL.Entities.Player", b =>
                {
                    b.Navigation("HostedEvents");

                    b.Navigation("Images");

                    b.Navigation("JoinedEvents");
                });

            modelBuilder.Entity("AirsoftMeetingGraphQL.Entities.Team", b =>
                {
                    b.Navigation("TeamCrew");
                });
#pragma warning restore 612, 618
        }
    }
}
