using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Data
{
    public class LocalDbContext : IdentityDbContext<User>
    {
        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BidList>(entity =>
            {
                entity.ToTable("BidList");

                entity.HasKey(e => e.BidListId);
                entity.Property(e => e.BidListId).HasColumnName("BidListId");
                entity.Property(e => e.Account).HasColumnName("Account");
                entity.Property(e => e.BidType).HasColumnName("BidType");
                entity.Property(e => e.BidQuantity).HasColumnName("BidQuantity");
                entity.Property(e => e.AskQuantity).HasColumnName("AskQuantity");
                entity.Property(e => e.Bid).HasColumnName("Bid");
                entity.Property(e => e.Ask).HasColumnName("Ask");
                entity.Property(e => e.Benchmark).HasColumnName("Benchmark");
                entity.Property(e => e.BidListDate).HasColumnName("BidListDate");
                entity.Property(e => e.Commentary).HasColumnName("Commentary");
                entity.Property(e => e.BidSecurity).HasColumnName("BidSecurity");
                entity.Property(e => e.BidStatus).HasColumnName("BidStatus");
                entity.Property(e => e.Trader).HasColumnName("Trader");
                entity.Property(e => e.Book).HasColumnName("Book");
                entity.Property(e => e.CreationName).HasColumnName("CreationName");
                entity.Property(e => e.CreationDate).HasColumnName("CreationDate");
                entity.Property(e => e.RevisionName).HasColumnName("RevisionName");
                entity.Property(e => e.RevisionDate).HasColumnName("RevisionDate");
                entity.Property(e => e.DealName).HasColumnName("DealName");
                entity.Property(e => e.DealType).HasColumnName("DealType");
                entity.Property(e => e.SourceListId).HasColumnName("SourceListId");
                entity.Property(e => e.Side).HasColumnName("Side");
            });

            builder.Entity<CurvePoint>(entity =>
            {
                entity.ToTable("CurvePoint");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.CurveId).HasColumnName("CurveId");
                entity.Property(e => e.AsOfDate).HasColumnName("AsOfDate");
                entity.Property(e => e.Term).HasColumnName("Term");
                entity.Property(e => e.CurvePointValue).HasColumnName("CurvePointValue");
                entity.Property(e => e.CreationDate).HasColumnName("CreationDate");
            });

            builder.Entity<Rating>(entity =>
            {
                entity.ToTable("Rating");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.MoodysRating).HasColumnName("MoodysRating");
                entity.Property(e => e.SandPRating).HasColumnName("SandPRating");
                entity.Property(e => e.FitchRating).HasColumnName("FitchRating");
                entity.Property(e => e.OrderNumber).HasColumnName("OrderNumber");
            });

            builder.Entity<RuleName>(entity =>
            {
                entity.ToTable("RuleName");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnName("Name");
                entity.Property(e => e.Description).HasColumnName("Description");
                entity.Property(e => e.Json).HasColumnName("Json");
                entity.Property(e => e.Template).HasColumnName("Template");
                entity.Property(e => e.SqlStr).HasColumnName("SqlStr");
                entity.Property(e => e.SqlPart).HasColumnName("SqlPart");
            });

            builder.Entity<Trade>(entity =>
            {
                entity.ToTable("Trade");

                entity.HasKey(e => e.TradeId);
                entity.Property(e => e.Account).HasColumnName("Account");
                entity.Property(e => e.AccountType).HasColumnName("AccountType");
                entity.Property(e => e.BuyQuantity).HasColumnName("BuyQuantity");
                entity.Property(e => e.SellQuantity).HasColumnName("SellQuantity");
                entity.Property(e => e.BuyPrice).HasColumnName("BuyPrice");
                entity.Property(e => e.SellPrice).HasColumnName("SellPrice");
                entity.Property(e => e.TradeDate).HasColumnName("TradeDate");
                entity.Property(e => e.TradeStatus).HasColumnName("TradeStatus");
                entity.Property(e => e.Trader).HasColumnName("Trader");
                entity.Property(e => e.Benchmark).HasColumnName("Benchmark");
                entity.Property(e => e.Book).HasColumnName("Book");
                entity.Property(e => e.CreationName).HasColumnName("CreationName");
                entity.Property(e => e.CreationDate).HasColumnName("CreationDate");
                entity.Property(e => e.RevisionName).HasColumnName("RevisionName");
                entity.Property(e => e.RevisionDate).HasColumnName("RevisionDate");
                entity.Property(e => e.DealName).HasColumnName("DealName");
                entity.Property(e => e.DealType).HasColumnName("DealType");
                entity.Property(e => e.SourceListId).HasColumnName("SourceListId");
                entity.Property(e => e.Side).HasColumnName("Side");
            });

            builder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.UserName)
                      .HasColumnName("UserName")
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(e => e.Fullname)
                      .HasColumnName("Fullname");
            });

        }

        public DbSet<BidList> BidLists { get; set; }
        public DbSet<CurvePoint> CurvePoints { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RuleName> RuleNames { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}