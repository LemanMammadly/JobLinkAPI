using JobLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobLink.DAL.Configurations;

public class CompanyIndusrtyConfiguration : IEntityTypeConfiguration<CompanyIndustry>
{
    public void Configure(EntityTypeBuilder<CompanyIndustry> builder)
    {
        builder.HasOne(ci => ci.Company)
            .WithMany(c => c.CompanyIndustries)
            .HasForeignKey(ci => ci.CompanyId);
        builder.HasOne(ci => ci.Industry)
            .WithMany(i => i.CompanyIndustries)
            .HasForeignKey(ci => ci.IndustryId);
    }
}

