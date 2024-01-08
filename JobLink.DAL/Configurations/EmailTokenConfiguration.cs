using JobLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobLink.DAL.Configurations;

public class EmailTokenConfiguration : IEntityTypeConfiguration<EmailToken>
{
    public void Configure(EntityTypeBuilder<EmailToken> builder)
    {
        builder.HasOne(e => e.AppUser)
            .WithOne(a => a.EmailToken)
            .HasForeignKey<EmailToken>(e => e.AppUserId);
    }
}

