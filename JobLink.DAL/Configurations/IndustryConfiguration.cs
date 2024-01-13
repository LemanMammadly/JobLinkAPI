using JobLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobLink.DAL.Configurations;

public class IndustryConfiguration : IEntityTypeConfiguration<Industry>
{
    public void Configure(EntityTypeBuilder<Industry> builder)
    {
        builder.Property(i => i.Name)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(i=>i.Logo)
            .IsRequired();
    }
}

