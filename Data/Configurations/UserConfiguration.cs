using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portfolioApi.Models;

namespace portfolioApi.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table mapping
            builder.ToTable("Users");

            // Primary Key
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            // Properties
            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("varchar(255)");

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasColumnType("text");

            // Unique Indexes
            builder.HasIndex(u => u.Username)
                .IsUnique()
                .HasDatabaseName("IX_Users_Username");

            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");
        }
    }
}
