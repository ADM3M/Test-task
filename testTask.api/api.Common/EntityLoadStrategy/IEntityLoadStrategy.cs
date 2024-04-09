using System.Linq.Expressions;

namespace api.Common.EntityLoadStrategy;

public interface IEntityLoadStrategy<TEntity>
{
    IReadOnlyCollection<Expression<Func<TEntity, object>>> Includes { get; }
}