using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilvernetJSONAPI.Models;

namespace SilvernetJSONAPI.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserResource>
    {
        public void Configure(EntityTypeBuilder<UserResource> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(u => u.Phone)
                .IsRequired()
                .HasMaxLength(12);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.IdNumber)
                .IsRequired()
                .HasMaxLength(9);

            builder.HasIndex(u => u.IdNumber)
                .IsUnique();

            builder.HasIndex(u => u.Phone)
                .IsUnique();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.CreationDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.TenantId).IsRequired();
        }
    }
}
