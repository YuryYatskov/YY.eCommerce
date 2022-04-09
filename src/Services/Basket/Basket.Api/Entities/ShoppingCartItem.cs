namespace Basket.Api.Entities
{
    /// <summary>
    /// A item in the shopping cart.
    /// </summary>
    public class ShoppingCartItem
    {
        /// <summary>
        /// A quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// A color.
        /// </summary>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// A price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// A product identifier.
        /// </summary>
        public string ProductId { get; set; } = string.Empty;

        /// <summary>
        /// A product name.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
    }
}