using Ordering.Domain.Common;
using System.Linq.Expressions;

namespace Ordering.Application.Contracts.Persistence
{
    /// <summary>
    /// The base repository.
    /// </summary>
    /// <typeparam name="T"> An entity type. </typeparam>
    public interface IAsyncRepository<T> where T : EntityBase
    {
        /// <summary>
        /// Get all members of an entity.
        /// </summary>
        /// <returns> Entity members. </returns>
        Task<IReadOnlyList<T>> GetAllAsync();

        /// <summary>
        /// Get all members of an entity by filter.
        /// </summary>
        /// <param name="predicate"> A filter condition. </param>
        /// <returns> Entity members. </returns>
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get all members of an entity by filter and other conditions.
        /// </summary>
        /// <param name="predicate"> A filter condition. </param>
        /// <param name="orderBy"> An ordering condition. </param>
        /// <param name="includeString"> Enable collections. </param>
        /// <param name="disableTracking"> Disable tracking. </param>
        /// <returns> Entity members. </returns>
         Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                        string? includeString = null,
                                        bool disableTracking = true);
        /// <summary>
        /// Get all members of an entity by filter and other conditions.
        /// </summary>
        /// <param name="predicate"> A filter condition. </param>
        /// <param name="orderBy"> An ordering condition. </param>
        /// <param name="includes"> Enable collections. </param>
        /// <param name="disableTracking"> Disable tracking. </param>
        /// <returns> Entity members. </returns>
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null,
                                       Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                       List<Expression<Func<T, object>>>? includes = null,
                                       bool disableTracking = true);
        /// <summary>
        /// Get an entity member by identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns> An entity member. </returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Add an entity member.
        /// </summary>
        /// <param name="entity"> An entity member. </param>
        /// <returns> An entity member. </returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Change an entity member.
        /// </summary>
        /// <param name="entity"> An entity member. </param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Delete an entity member.
        /// </summary>
        /// <param name="entity"> An entity member. </param>
        Task DeleteAsync(T entity);
    }
}
