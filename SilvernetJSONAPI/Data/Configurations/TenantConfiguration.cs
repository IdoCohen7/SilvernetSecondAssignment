using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilvernetJSONAPI.Models;

namespace SilvernetJSONAPI.Data.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<TenantResource>
    {
        public void Configure(EntityTypeBuilder<TenantResource> builder)
        {
            builder.ToTable("Tenants");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(t => t.Email)
                .IsUnique();

            builder.Property(t => t.Phone)
                .IsRequired()
                .HasMaxLength(12);

            builder.Property(t => t.CreationDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();


            builder.HasMany(t => t.Users)
                .WithOne(u => u.Tenant)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
