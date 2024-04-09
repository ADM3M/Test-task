using System.Linq.Expressions;

namespace api.Common.EntityLoadStrategy;

public class EntityLoadStrategy<TEntity> : IEntityLoadStrategy<TEntity>
{
    public IReadOnlyCollection<Expression<Func<TEntity, object>>> Includes { get; }


    public EntityLoadStrategy(params Expression<Func<TEntity, object>>[] includes)
    {
        Includes = includes;
    }
}