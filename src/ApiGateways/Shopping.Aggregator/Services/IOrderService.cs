using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    /// <summary>
    /// The order service.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Get an order by user name.
        /// </summary>
        /// <param name="userName"> A user name. </param>
        /// <returns> Orders. </returns>
        Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
    }
}
