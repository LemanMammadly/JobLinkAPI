using JobLink.Core.Entities;
using JobLink.DAL.Contexts;
using JobLink.DAL.Repositories.Interfaces;

namespace JobLink.DAL.Repositories.Implements;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    public CompanyRepository(AppDbContext context) : base(context)
    {
    }
}

