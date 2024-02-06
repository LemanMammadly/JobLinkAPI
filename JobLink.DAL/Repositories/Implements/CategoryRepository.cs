using JobLink.Core.Entities;
using JobLink.DAL.Contexts;
using JobLink.DAL.Repositories.Interfaces;

namespace JobLink.DAL.Repositories.Implements;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}

