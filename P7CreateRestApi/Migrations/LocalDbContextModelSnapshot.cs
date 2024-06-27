﻿// <auto-generated />
using System;
using Dot.Net.WebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace P7CreateRestApi.Migrations
{
    [DbContext(typeof(LocalDbContext))]
    partial class LocalDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

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
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PasswordHash");

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

                    b.ToTable("User", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
