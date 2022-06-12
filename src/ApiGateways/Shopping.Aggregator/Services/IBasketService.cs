using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    /// <summary>
    /// The basket service.
    /// </summary>
    public interface IBasketService
    {
        /// <summary>
        /// Get basket by a user name.
        /// </summary>
        /// <param name="userName"> A user name. </param>
        /// <returns> A basket. </returns>
        Task<BasketModel> GetBasket(string userName);
    }
}
