using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SGO.Infrastructure.Persistence.Configurations
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd();
            builder.Property(u => u.Matricula)
                   .IsRequired();
            builder.Property(u => u.Username)
                   .HasMaxLength(50)
                   .IsRequired();
            builder.Property(u => u.PasswordHash)
                   .HasMaxLength(256)
                   .IsRequired();
            builder.Property(u => u.Role)
                   .HasMaxLength(5)
                   .IsRequired();
            builder.HasIndex(u => u.Username)
                   .IsUnique();
        }
    }
}
