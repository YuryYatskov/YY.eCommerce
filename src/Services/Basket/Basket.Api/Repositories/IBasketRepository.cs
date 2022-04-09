using Basket.Api.Entities;

namespace Basket.Api.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetBasketAsync(string userName);

        Task<ShoppingCart?> UpdateBasketAsync(ShoppingCart basket);

        Task DeleteBasketAsync(string userName);
    }
}
