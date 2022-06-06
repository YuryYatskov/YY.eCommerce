using Ordering.Domain.Entities;

namespace Ordering.Application.Contracts.Persistence
{
    /// <summary>
    /// The order repository.
    /// </summary>
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        /// <summary>
        /// Get orders by an user name.
        /// </summary>
        /// <param name="userName1"></param>
        /// <returns></returns>
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName1);
    }
}
