using JobLink.Core.Entities;
using JobLink.DAL.Contexts;
using JobLink.DAL.Repositories.Interfaces;

namespace JobLink.DAL.Repositories.Implements;

public class AbilityRepository : Repository<Ability>, IAbilityRepository
{
    public AbilityRepository(AppDbContext context) : base(context)
    {
    }
}

