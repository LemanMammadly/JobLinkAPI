using System.Linq.Expressions;
using JobLink.Core.Entities.CommonEntities;
using Microsoft.EntityFrameworkCore;

namespace JobLink.DAL.Repositories.Interfaces;

public interface IRepository<T> where T : BaseEntity , new()
{
    DbSet<T> Table { get; }
    IQueryable<T> GetAll(params string[] includes);
    IQueryable<T> FindAll(Expression<Func<T,bool>> expression, params string[] includes);
    Task<T> GetByIdAsync(int id,params string[] includes);
    Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, params string[] inlcudes);
    Task<bool> IsExistAsync(Expression<Func<T, bool>> expression);
    Task CreateAsync(T entity);
    Task DeleteAsync(int id);
    void Delete(T entity);
    void SoftDelete(T entity);
    void ReverteSoftDelete(T entity);
    Task SaveAsync();
}

