namespace Shopping.Aggregator.Models
{
    /// <summary>
    /// A catalog model.
    /// </summary>
    public class CatalogModel
    {
        /// <summary>
        /// Identifier of product.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Name of product.
        /// </summary>
        public string Name { get; set; } = string.Empty;

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

        /// <summary>
        /// Product price.
        /// </summary>
        public decimal Price { get; set; }
    }
}