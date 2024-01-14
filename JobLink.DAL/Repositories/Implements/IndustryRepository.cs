using JobLink.Core.Entities;
using JobLink.DAL.Contexts;
using JobLink.DAL.Repositories.Interfaces;

namespace JobLink.DAL.Repositories.Implements;

public class IndustryRepository : Repository<Industry>, IIndustryRepository
{
    public IndustryRepository(AppDbContext context) : base(context)
    {
    }
}

