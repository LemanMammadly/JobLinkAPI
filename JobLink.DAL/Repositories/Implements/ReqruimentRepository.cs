using JobLink.Core.Entities;
using JobLink.DAL.Contexts;
using JobLink.DAL.Repositories.Interfaces;

namespace JobLink.DAL.Repositories.Implements;

public class ReqruimentRepository : Repository<Reqruiment>, IReqruimentRepository
{
    public ReqruimentRepository(AppDbContext context) : base(context)
    {
    }
}

