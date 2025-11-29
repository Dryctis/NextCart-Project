using NexCart.Domain.Common.Entities;
using System.Linq.Expressions;

namespace NexCart.Application.Common.Interfaces;


public interface IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : notnull
{
 
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);


    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);


    Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

 
    Task<TEntity?> GetSingleAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

  
    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

  
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

   
    void Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);


    void RemoveRange(IEnumerable<TEntity> entities);
}