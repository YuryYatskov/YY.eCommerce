namespace Discount.Api.Entities
{
    /// <summary>
    /// A discount coupon.
    /// </summary>
    public class Coupon
    {
        /// <summary>
        /// A identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A product name.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// A description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Discount amount.
        /// </summary>
        public int Amount { get; set; }
    }
}
