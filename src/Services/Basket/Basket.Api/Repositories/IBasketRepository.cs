using Basket.Api.Entities;

namespace Basket.Api.Repositories
{
    /// <summary>
    /// A basket repository.
    /// </summary>
    public interface IBasketRepository
    {
        /// <summary>
        /// Get a basket.
        /// </summary>
        /// <param name="userName"> A user name. </param>
        Task<ShoppingCart> GetBasketAsync(string userName);

        /// <summary>
        /// Add or update a basket.
        /// </summary>
        /// <param name="basket"> A basket. </param>
        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket);

        /// <summary>
        /// Delete a basket.
        /// </summary>
        /// <param name="userName"> A user name. </param>
        Task DeleteBasketAsync(string userName);
    }
}