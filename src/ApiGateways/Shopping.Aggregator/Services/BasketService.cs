using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    /// <inheritdoc  cref="IBasketService"/>
    public class BasketService : IBasketService
    {
        /// <inheritdoc/>
        public Task<BasketModel> GetBasket(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
