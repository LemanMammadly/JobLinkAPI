using JobLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobLink.DAL.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.Property(c => c.Name)
            .HasMaxLength(70)
            .IsRequired();
        builder.Property(c => c.Description)
            .IsRequired();
        builder.Property(c => c.Email)
            .IsRequired();
        builder.Property(c => c.Phone)
            .IsRequired();
        builder.Property(c => c.Address)
            .IsRequired();
        builder.Property(c => c.CreateDate)
            .HasDefaultValueSql("DATEADD(hour, 4, GETUTCDATE())")
            .IsRequired();
    }
}

