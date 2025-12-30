using Microsoft.EntityFrameworkCore;
using SilvernetJSONAPI.Models;
using SilvernetJSONAPI.Data.Configurations;

namespace SilvernetJSONAPI.Data
{
    public class ReadDbContext : DbContext
    {

        private readonly ILogger<ReadDbContext> _logger;
        public ReadDbContext(DbContextOptions<ReadDbContext> options, ILogger<ReadDbContext> logger) : base(options) {
        
            _logger = logger;
            /*
             * used to test in development
            _logger.LogWarning("### READ CONTEXT CONSTRUCTED ###");
            */
        }

        public DbSet<TenantResource> Tenants => Set<TenantResource>();
        public DbSet<UserResource> Users => Set<UserResource>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TenantConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }


    }
}
