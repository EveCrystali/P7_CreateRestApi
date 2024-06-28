﻿// <auto-generated />
using System;
using Dot.Net.WebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace P7CreateRestApi.Migrations
{
    [DbContext(typeof(LocalDbContext))]
    [Migration("20240628082301_M4")]
    partial class M4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Dot.Net.WebApi.Controllers.Domain.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FitchRating")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("FitchRating");

                    b.Property<string>("MoodysRating")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("MoodysRating");

                    b.Property<byte?>("OrderNumber")
                        .HasColumnType("tinyint")
                        .HasColumnName("OrderNumber");

                    b.Property<string>("SandPRating")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("SandPRating");

                    b.HasKey("Id");

                    b.ToTable("Rating", (string)null);
                });

            modelBuilder.Entity("Dot.Net.WebApi.Controllers.RuleName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Description");

                    b.Property<string>("Json")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Json");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.Property<string>("SqlPart")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("SqlPart");

                    b.Property<string>("SqlStr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("SqlStr");

                    b.Property<string>("Template")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Template");

                    b.HasKey("Id");

                    b.ToTable("RuleName", (string)null);
                });

            modelBuilder.Entity("Dot.Net.WebApi.Domain.BidList", b =>
                {
                    b.Property<int>("BidListId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("BidListId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BidListId"));

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Account");

                    b.Property<double?>("Ask")
                        .HasColumnType("float")
                        .HasColumnName("Ask");

                    b.Property<double?>("AskQuantity")
                        .HasColumnType("float")
                        .HasColumnName("AskQuantity");

                    b.Property<string>("Benchmark")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Benchmark");

                    b.Property<double?>("Bid")
                        .HasColumnType("float")
                        .HasColumnName("Bid");

                    b.Property<DateTime?>("BidListDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("BidListDate");

                    b.Property<double?>("BidQuantity")
                        .HasColumnType("float")
                        .HasColumnName("BidQuantity");

                    b.Property<string>("BidSecurity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("BidSecurity");

                    b.Property<string>("BidStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("BidStatus");

                    b.Property<string>("BidType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("BidType");

                    b.Property<string>("Book")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Book");

                    b.Property<string>("Commentary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Commentary");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationDate");

                    b.Property<string>("CreationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CreationName");

                    b.Property<string>("DealName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("DealName");

                    b.Property<string>("DealType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("DealType");

                    b.Property<DateTime?>("RevisionDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("RevisionDate");

                    b.Property<string>("RevisionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("RevisionName");

                    b.Property<string>("Side")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Side");

                    b.Property<string>("SourceListId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("SourceListId");

                    b.Property<string>("Trader")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Trader");

                    b.HasKey("BidListId");

                    b.ToTable("BidList", (string)null);
                });

            modelBuilder.Entity("Dot.Net.WebApi.Domain.CurvePoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("AsOfDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("AsOfDate");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationDate");

                    b.Property<byte?>("CurveId")
                        .HasColumnType("tinyint")
                        .HasColumnName("CurveId");

                    b.Property<double?>("CurvePointValue")
                        .HasColumnType("float")
                        .HasColumnName("CurvePointValue");

                    b.Property<double?>("Term")
                        .HasColumnType("float")
                        .HasColumnName("Term");

                    b.HasKey("Id");

                    b.ToTable("CurvePoint", (string)null);
                });

            modelBuilder.Entity("Dot.Net.WebApi.Domain.Trade", b =>
                {
                    b.Property<int>("TradeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TradeId"));

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Account");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("AccountType");

                    b.Property<string>("Benchmark")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Benchmark");

                    b.Property<string>("Book")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Book");

                    b.Property<double?>("BuyPrice")
                        .HasColumnType("float")
                        .HasColumnName("BuyPrice");

                    b.Property<double?>("BuyQuantity")
                        .HasColumnType("float")
                        .HasColumnName("BuyQuantity");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationDate");

                    b.Property<string>("CreationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CreationName");

                    b.Property<string>("DealName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("DealName");

                    b.Property<DateTime?>("RevisionDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("RevisionDate");

                    b.Property<string>("RevisionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("RevisionName");

                    b.Property<double?>("SellPrice")
                        .HasColumnType("float")
                        .HasColumnName("SellPrice");

                    b.Property<double?>("SellQuantity")
                        .HasColumnType("float")
                        .HasColumnName("SellQuantity");

                    b.Property<DateTime?>("TradeDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("TradeDate");

                    b.Property<string>("TradeSecurity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TradeStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("TradeStatus");

                    b.Property<string>("Trader")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Trader");

                    b.HasKey("TradeId");

                    b.ToTable("Trade", (string)null);
                });

            modelBuilder.Entity("Dot.Net.WebApi.Domain.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Fullname");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("UserName");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Dot.Net.WebApi.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Dot.Net.WebApi.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dot.Net.WebApi.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Dot.Net.WebApi.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
