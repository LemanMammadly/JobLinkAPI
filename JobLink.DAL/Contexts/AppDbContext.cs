using Microsoft.EntityFrameworkCore;

namespace JobLink.DAL.Contexts;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options) {}
}

