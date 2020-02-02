using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WeLoveTaxes.Domain.Seed
{
    public interface IRepository<TEntity> where TEntity : IAggregateRoot
    {
        Task<IEnumerable<TEntity>> All();

        Task<IEnumerable<TEntity>> AllInclude(params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> FindByInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> FindBy(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FindFirstOrDefaultBy(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FindByKey(Guid id);

        Task<TEntity> Add(TEntity entity);

        Task Update(TEntity entity);

        Task PartialUpdate(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> properties);

        Task Delete(Guid id);
    }
}