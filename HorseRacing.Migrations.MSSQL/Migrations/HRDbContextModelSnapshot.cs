﻿// <auto-generated />
using System;
using HorseRacing.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HorseRacing.Migrations.MSSQL.Migrations
{
    [DbContext(typeof(HRDbContext))]
    partial class HRDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HorseRacing.Domain.EventLogAggregate.EventLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EventTitle")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int?>("EventType")
                        .HasColumnType("int");

                    b.Property<string>("Exception")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InitiatorInfoText")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Level")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MessageTemplate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Properties")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Subsystem")
                        .HasColumnType("int");

                    b.Property<int?>("TechProcess")
                        .HasColumnType("int");

                    b.Property<DateTime?>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("EventLog", (string)null);
                });

            modelBuilder.Entity("HorseRacing.Domain.GameAggregate.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ChangedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateChanged")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChangedUserId");

                    b.HasIndex("CreatedUserId");

                    b.ToTable("Games", (string)null);
                });

            modelBuilder.Entity("HorseRacing.Domain.UserAggregate.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ChangedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateChanged")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("ChangedUserId");

                    b.HasIndex("CreatedUserId");

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("8223998a-318f-460c-9464-1164ee56cb46"),
                            DateCreated = new DateTime(2024, 12, 31, 21, 0, 0, 0, DateTimeKind.Utc),
                            Email = "admin@race.ru",
                            FirstName = "Администратор",
                            IsRemoved = false,
                            LastName = "Системы",
                            Password = "82E3B4B3D57F6D4112A02310EA2E8F9517BC19BD6EBF1EC95E0C7AA961B3B3F2AE24BB2CA278CB190D24B55241AC893C9E590717106F3FB18070",
                            Phone = "79001112233",
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("HorseRacing.Domain.EventLogAggregate.EventLog", b =>
                {
                    b.HasOne("HorseRacing.Domain.UserAggregate.User", null)
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("HorseRacing.Domain.GameAggregate.Game", b =>
                {
                    b.HasOne("HorseRacing.Domain.UserAggregate.User", null)
                        .WithMany()
                        .HasForeignKey("ChangedUserId");

                    b.HasOne("HorseRacing.Domain.UserAggregate.User", null)
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.OwnsMany("HorseRacing.Domain.GameAggregate.Entities.GameDeckCard", "GameDeckCards", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("CardOrder")
                                .HasColumnType("int");

                            b1.Property<int>("CardRank")
                                .HasColumnType("int");

                            b1.Property<int>("CardSuit")
                                .HasColumnType("int");

                            b1.Property<Guid>("GameId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Zone")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("GameId");

                            b1.ToTable("GameDeckCards", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("GameId");
                        });

                    b.OwnsMany("HorseRacing.Domain.GameAggregate.Entities.GameEvent", "GameEvents", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("CardOrder")
                                .HasColumnType("int");

                            b1.Property<int>("CardRank")
                                .HasColumnType("int");

                            b1.Property<int>("CardSuit")
                                .HasColumnType("int");

                            b1.Property<DateTime>("EventDate")
                                .HasColumnType("datetime2");

                            b1.Property<int>("EventType")
                                .HasColumnType("int");

                            b1.Property<Guid>("GameId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("HorseSuit")
                                .HasColumnType("int");

                            b1.Property<int>("Position")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("GameId");

                            b1.ToTable("GameEvents", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("GameId");
                        });

                    b.OwnsMany("HorseRacing.Domain.GameAggregate.Entities.GameHorsePosition", "GameHorsePositions", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("GameId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("HorseSuit")
                                .HasColumnType("int");

                            b1.Property<int>("Position")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("GameId");

                            b1.ToTable("GameHorsePositions", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("GameId");
                        });

                    b.OwnsMany("HorseRacing.Domain.GameAggregate.Entities.GamePlayer", "GamePlayers", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("BetAmount")
                                .HasColumnType("int");

                            b1.Property<int>("BetSuit")
                                .HasColumnType("int");

                            b1.Property<Guid>("GameId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("Id");

                            b1.HasIndex("GameId");

                            b1.HasIndex("UserId");

                            b1.ToTable("GamePlayers", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("GameId");

                            b1.HasOne("HorseRacing.Domain.UserAggregate.User", null)
                                .WithMany()
                                .HasForeignKey("UserId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();
                        });

                    b.OwnsOne("HorseRacing.Domain.GameAggregate.Entities.GameResult", "GameResult", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("BetSuit")
                                .HasColumnType("int");

                            b1.Property<Guid>("GameId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Position")
                                .HasColumnType("int");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("Id");

                            b1.HasIndex("GameId")
                                .IsUnique();

                            b1.HasIndex("UserId");

                            b1.ToTable("GameResults", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("GameId");

                            b1.HasOne("HorseRacing.Domain.UserAggregate.User", null)
                                .WithMany()
                                .HasForeignKey("UserId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();
                        });

                    b.Navigation("GameDeckCards");

                    b.Navigation("GameEvents");

                    b.Navigation("GameHorsePositions");

                    b.Navigation("GamePlayers");

                    b.Navigation("GameResult");
                });

            modelBuilder.Entity("HorseRacing.Domain.UserAggregate.User", b =>
                {
                    b.HasOne("HorseRacing.Domain.UserAggregate.User", null)
                        .WithMany()
                        .HasForeignKey("ChangedUserId");

                    b.HasOne("HorseRacing.Domain.UserAggregate.User", null)
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.OwnsOne("HorseRacing.Domain.UserAggregate.Entities.Account", "Account", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Balance")
                                .HasColumnType("int");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("Id");

                            b1.HasIndex("UserId")
                                .IsUnique();

                            b1.ToTable("Accounts", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UserId");

                            b1.HasData(
                                new
                                {
                                    Id = new Guid("1223998a-318f-460c-9464-1164ee56cb46"),
                                    Balance = 0,
                                    UserId = new Guid("8223998a-318f-460c-9464-1164ee56cb46")
                                });
                        });

                    b.Navigation("Account");
                });
#pragma warning restore 612, 618
        }
    }
}
