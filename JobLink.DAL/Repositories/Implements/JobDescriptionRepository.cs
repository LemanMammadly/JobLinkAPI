using JobLink.Core.Entities;
using JobLink.DAL.Contexts;
using JobLink.DAL.Repositories.Interfaces;

namespace JobLink.DAL.Repositories.Implements;

public class JobDescriptionRepository : Repository<JobDescription>, IJobDescriptionRepository
{
    public JobDescriptionRepository(AppDbContext context) : base(context)
    {
    }
}

