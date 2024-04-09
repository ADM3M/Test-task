using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api.Common.EntityLoadStrategy;

namespace api.Repositories.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, IEntityLoadStrategy<TEntity>? loadStrategy = null);

    Task<IReadOnlyCollection<TEntity>> GetWhereAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        IEntityLoadStrategy<TEntity>? loadStrategy = null,
        bool asNoTracking = false);

    Task<TEntity> AddAsync(TEntity entity);

    Task AddRangeAsync(IReadOnlyCollection<TEntity> entities);

    void Remove(TEntity patient);

    Task<bool> SaveChangesAsync();
}