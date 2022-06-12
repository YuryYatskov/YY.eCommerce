using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    /// <inheritdoc cref="IOrderService"/>
    public class OrderService : IOrderService
    {
        /// <inheritdoc/>
        public Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
