using System.Collections.Generic;

namespace api.Repositories.EntityOperationResult;

public class EntityOperationResult<TEntity, TError>
{
    public bool IsSuccessful => Errors.Count == 0;

    public TEntity Entity { get; set; }

    public IReadOnlyCollection<TError> Errors { get; set; }


    private EntityOperationResult(TEntity entity = default(TEntity), IReadOnlyCollection<TError>? errors = null)
    {
        Entity = entity;
        Errors = errors ?? new List<TError>();
    }


    public static EntityOperationResult<TEntity, TError> CreateSuccessful(TEntity entity)
    {
        return new EntityOperationResult<TEntity, TError>(entity: entity);
    }

    public static EntityOperationResult<TEntity, TError> CreateUnsuccessful(params TError[] errors)
    {
        return new EntityOperationResult<TEntity, TError>(errors: errors);
    }
    
    public static implicit operator EntityOperationResult<TEntity, TError>(TEntity entity)
    {
        return CreateSuccessful(entity);
    }

    public static implicit operator EntityOperationResult<TEntity, TError>(TError error)
    {
        return CreateUnsuccessful(error);
    }
}