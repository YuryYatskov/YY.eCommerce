namespace Shopping.Aggregator.Models
{
    /// <summary>
    /// A basket item.
    /// </summary>
    public class BasketItemExtendedModel
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


        #region Product related additional fields

        /// <summary>
        /// Category of product.
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Summary of product characteristics.
        /// </summary>
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// Product description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Picture or photo of the product.
        /// </summary>
        public string ImageFile { get; set; } = string.Empty;

        #endregion
    }
}