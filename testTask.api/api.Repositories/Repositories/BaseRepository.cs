using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api.Common.EntityLoadStrategy;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;


    protected BaseRepository(DbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TEntity>();
    }


    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, IEntityLoadStrategy<TEntity>? loadStrategy = null)
    {
        return await GetQuery(loadStrategy).FirstOrDefaultAsync(predicate);
    }
    
    public async Task<IReadOnlyCollection<TEntity>> GetWhereAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        IEntityLoadStrategy<TEntity>? loadStrategy = null,
        bool asNoTracking = false)
    {
        var query = GetQuery(loadStrategy);
        
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }
        
        return await query.ToListAsync();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        var newEntity = await DbSet.AddAsync(entity);

        return newEntity.Entity;
    }

    public async Task AddRangeAsync(IReadOnlyCollection<TEntity> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public virtual void Remove(TEntity patient)
    {
        DbSet.Attach(patient);
        DbSet.Remove(patient);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await DbContext.SaveChangesAsync() > 0;
    }


    private IQueryable<TEntity> GetQuery(IEntityLoadStrategy<TEntity>? loadStrategy = null)
    {
        return loadStrategy is null ? DbSet : MergeIncludes(DbSet, loadStrategy.Includes);
    }

    private static IQueryable<TEntity> MergeIncludes(
        IQueryable<TEntity> query,
        IReadOnlyCollection<Expression<Func<TEntity, object>>> loadStrategyIncludes)
    {
        return loadStrategyIncludes.Aggregate(query, (current, include) => current.Include(include));
    }
}