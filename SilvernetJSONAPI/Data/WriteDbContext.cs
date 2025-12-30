using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SilvernetJSONAPI.Models;
using SilvernetJSONAPI.Data.Configurations;

namespace SilvernetJSONAPI.Data
{
    public class WriteDbContext : DbContext
    {
        private readonly ILogger<WriteDbContext> _logger;

        public WriteDbContext(DbContextOptions<WriteDbContext> options, ILogger<WriteDbContext> logger) : base(options) {
            _logger = logger;
            /*
             * was used in developement
            _logger.LogWarning("### WRITE CONTEXT CONSTRUCTED ###");
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

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

       
    }
}
