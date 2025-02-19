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
                            Password = "2A9BEAAD6666982A470776F098D3D9B6C41D1AB8D3286E7DC710DC6CE7393DB15BFDF9D0CE2C153E39A64D4FD9F04DC68FE5345BFCAA4ACB3F03",
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

            modelBuilder.Entity("HorseRacing.Domain.UserAggregate.User", b =>
                {
                    b.HasOne("HorseRacing.Domain.UserAggregate.User", null)
                        .WithMany()
                        .HasForeignKey("ChangedUserId");

                    b.HasOne("HorseRacing.Domain.UserAggregate.User", null)
                        .WithMany()
                        .HasForeignKey("CreatedUserId");
                });
#pragma warning restore 612, 618
        }
    }
}
