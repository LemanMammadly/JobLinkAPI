using JobLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobLink.DAL.Configurations;

public class ReqruimentConfiguration : IEntityTypeConfiguration<Reqruiment>
{
    public void Configure(EntityTypeBuilder<Reqruiment> builder)
    {
        builder.Property(r => r.Text)
            .IsRequired();
        builder.HasOne(r => r.Advertisement)
            .WithMany(a => a.Reqruiments)
            .HasForeignKey(r => r.AdvertisementId);
            
    }
}

