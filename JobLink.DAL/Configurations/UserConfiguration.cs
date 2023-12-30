using JobLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobLink.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(u => u.Name)
            .HasMaxLength(25)
            .IsRequired();
        builder.Property(u => u.Surname)
            .HasMaxLength(25)
            .IsRequired();
    }
}

