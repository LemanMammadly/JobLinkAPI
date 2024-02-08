using System.Reflection;
using JobLink.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobLink.DAL.Contexts;

public class AppDbContext:IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options) {}

    public DbSet<EmailToken> EmailTokens { get; set; }
    public DbSet<Industry> Industries { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyIndustry> CompanyIndustries { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Advertisement> Advertisements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}




