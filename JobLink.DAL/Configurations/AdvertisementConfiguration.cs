using JobLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobLink.DAL.Configurations;

public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
{
    public void Configure(EntityTypeBuilder<Advertisement> builder)
    {
        builder.Property(a => a.Title)
            .IsRequired();
        builder.Property(a => a.City)
            .IsRequired();
        builder.Property(a => a.WorkGraphic)
            .IsRequired();
        builder.Property(a => a.CreateDate)
            .HasDefaultValueSql("DATEADD(hour, 4, GETUTCDATE())")
            .IsRequired();
        builder.Property(a => a.EndDate)
            .IsRequired();
        builder.Property(a => a.JobDesc)
            .IsRequired();
        builder.Property(a => a.Reqruiment)
            .IsRequired();
        builder.HasOne(a => a.Category)
            .WithMany(c => c.Advertisements)
            .HasForeignKey(a => a.CategoryId);
        builder.HasOne(a => a.Company)
            .WithMany(c => c.Advertisements)
            .HasForeignKey(a => a.CompanyId);
    }
}

