using JobLink.Core.Entities;
using JobLink.DAL.Contexts;
using JobLink.DAL.Repositories.Interfaces;

namespace JobLink.DAL.Repositories.Implements;

public class AdvertisementRepository : Repository<Advertisement>, IAdvertisementRepository
{
    public AdvertisementRepository(AppDbContext context) : base(context)
    {
    }
}

