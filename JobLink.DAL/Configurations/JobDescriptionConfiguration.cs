using JobLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobLink.DAL.Configurations;

public class JobDescriptionConfiguration : IEntityTypeConfiguration<JobDescription>
{
    public void Configure(EntityTypeBuilder<JobDescription> builder)
    {
        builder.Property(j => j.Description)
            .IsRequired();
        builder.HasOne(j => j.Advertisement)
            .WithMany(a => a.JobDescriptions)
            .HasForeignKey(j => j.AdvertisementId);
    }
}

