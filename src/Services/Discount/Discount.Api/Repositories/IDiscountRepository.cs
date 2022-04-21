using Discount.Api.Entities;

namespace Discount.Api.Repositories
{
    /// <summary>
    /// A discount repository.
    /// </summary>
    public interface IDiscountRepository
    {
        /// <summary>
        /// Get a product discount.
        /// </summary>
        /// <param name="productName"> A product name. </param>
        /// <returns> A product discount. </returns>
        Task<Coupon> GetDiscountAsync(string productName);

        /// <summary>
        /// Get a product discount by identifier.
        /// </summary>
        /// <param name="id"> A product identifier. </param>
        /// <returns> A product discount. </returns>
        Task<Coupon?> GetDiscountAsync(int id);

        /// <summary>
        /// Create product discount.
        /// </summary>
        /// <param name="coupon"> A discount coupon. </param>
        /// <returns> A creation result. </returns>
        Task<bool> CreateDiscountAsync(Coupon coupon);

        /// <summary>
        /// Update product discount.
        /// </summary>
        /// <param name="coupon"> A discount coupon. </param>
        /// <returns> A update result. </returns>
        Task<bool> UpdateDiscountAsync(Coupon coupon);

        /// <summary>
        /// Delete product discount.
        /// </summary>
        /// <param name="productName"> A product name. </param>
        /// <returns> A deletion result. </returns>
        Task<bool> DeleteDiscountAsync(string productName);
    }
}