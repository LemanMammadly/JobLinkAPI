using JobLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobLink.DAL.Configurations;

public class AdvertisementAbilitiesConfiguration : IEntityTypeConfiguration<AdvertisementAbilities>
{
    public void Configure(EntityTypeBuilder<AdvertisementAbilities> builder)
    {
        builder.HasOne(a => a.Ability)
            .WithMany(a => a.AdvertisementAbilities)
            .HasForeignKey(a => a.AbilityId);
        builder.HasOne(a => a.Advertisement)
            .WithMany(a => a.AdvertisementAbilities)
            .HasForeignKey(a => a.AdvertisementId);
    }
}

