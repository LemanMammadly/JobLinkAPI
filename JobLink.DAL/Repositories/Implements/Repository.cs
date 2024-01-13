using System.Linq.Expressions;
using JobLink.Core.Entities.CommonEntities;
using JobLink.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobLink.DAL.Repositories.Implements;

public class Repository<T> : IRepository<T> where T : BaseEntity, new()
{
    public DbSet<T> Table => throw new NotImplementedException();

    public Task CreateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(T entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> FindAll(Expression<Func<T, bool>> expression, params string[] includes)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> GetAll(params string[] includes)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetByIdAsync(int id, params string[] includes)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, params string[] inlcudes)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsExistAsync(Expression<Func<T, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public void ReverteSoftDelete(T entity)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync()
    {
        throw new NotImplementedException();
    }

    public void SoftDelete(T entity)
    {
        throw new NotImplementedException();
    }
}

