using Microsoft.EntityFrameworkCore;
using Promrub.Services.API.Entities;

namespace Promrub.Services.API.PromServiceDbContext
{
    public class PromrubDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public PromrubDbContext(IConfiguration configuration, DbContextOptions<PromrubDbContext> options) : base(options)
        {
            Configuration = configuration;
        }
        public DbSet<ProvinceEntity>? Provinces { get; set; }
        public DbSet<DistrictEntity>? District { get; set; }
        public DbSet<SubDistrictEntity>? SubDistrict { get; set; }
        public DbSet<BankEntity>? Banks { get; set; }
        public DbSet<PaymentMethodEntity>? PaymentMethods { get; set; }
        public DbSet<OrganizationEntity>? Organizations { get; set; }
        public DbSet<ApiKeyEntity>? ApiKeys { get; set; }
        public DbSet<RoleEntity>? Roles { get; set; }
        public DbSet<UserEntity>? Users { get; set; }
        public DbSet<OrganizationUserEntity>? OrganizationUsers { get; set; }
        public DbSet<PaymentTransactionEntity>? PaymentTransactions { get; set; }
        public DbSet<PaymentChannelEntity> paymentChannels { get; set; }
        public DbSet<PaymentTransactionItemEntity>? PaymentTransactionItems { get; set; }
        public DbSet<ReceiptNumbersEntity>? ReceiptNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProvinceEntity>();
            modelBuilder.Entity<DistrictEntity>();
            modelBuilder.Entity<SubDistrictEntity>();
            modelBuilder.Entity<BankEntity>();
            modelBuilder.Entity<PaymentMethodEntity>();
            modelBuilder.Entity<OrganizationEntity>();
            modelBuilder.Entity<ApiKeyEntity>();
            modelBuilder.Entity<RoleEntity>();
            modelBuilder.Entity<UserEntity>();
            modelBuilder.Entity<OrganizationUserEntity>();
            modelBuilder.Entity<PaymentTransactionEntity>();
            modelBuilder.Entity<PaymentChannelEntity>();
            modelBuilder.Entity<PaymentTransactionItemEntity>();
            modelBuilder.Entity<ReceiptNumbersEntity>();
        }
    }
}
