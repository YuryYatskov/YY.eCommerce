using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _distributedCache;

        public BasketRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }

        public async Task<ShoppingCart?> GetBasketAsync(string userName)
        {
            var basket = await _distributedCache.GetStringAsync(userName);

            if(string.IsNullOrEmpty(basket))
                return null;

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart?> UpdateBasketAsync(ShoppingCart basket)
        {
            await _distributedCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize<ShoppingCart>(basket));
            return await GetBasketAsync(basket.UserName);
        }

        public Task DeleteBasketAsync(string userName)
        {
            return _distributedCache.RemoveAsync(userName);
        }
    }
}
