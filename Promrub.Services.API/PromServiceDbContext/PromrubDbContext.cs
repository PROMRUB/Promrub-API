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

        public DbSet<OrganizationEntity>? Organizations { get; set; }
        public DbSet<ApiKeyEntity>? ApiKeys { get; set; }
        public DbSet<RoleEntity>? Roles { get; set; }
        public DbSet<UserEntity>? Users { get; set; }
        public DbSet<OrganizationUserEntity>? OrganizationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrganizationEntity>();
            modelBuilder.Entity<ApiKeyEntity>();
            modelBuilder.Entity<RoleEntity>();
            modelBuilder.Entity<UserEntity>();
            modelBuilder.Entity<OrganizationUserEntity>();
        }
    }
}
