using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Api.Repositories
{
    /// <inheritdoc cref="IBasketRepository"/>
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _distributedCache;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="distributedCache"> A Distributed values cache. </param>
        public BasketRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }

        /// <inheritdoc/>
        public async Task<ShoppingCart> GetBasketAsync(string userName)
        {
            var basket = await _distributedCache.GetStringAsync(userName);

            ShoppingCart? shoppingCart = null;

            if (!string.IsNullOrEmpty(basket))
                shoppingCart = JsonSerializer.Deserialize<ShoppingCart>(basket);

            return shoppingCart ?? new ShoppingCart(userName);
        }

        /// <inheritdoc/>
        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
        {
            await _distributedCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize<ShoppingCart>(basket));
            return await GetBasketAsync(basket.UserName);
        }

        /// <inheritdoc/>
        public Task DeleteBasketAsync(string userName)
        {
            return _distributedCache.RemoveAsync(userName);
        }
    }
}
