using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Entities
{
    /// <summary>
    /// A product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Identifier of product.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonRequired]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Name of product.
        /// </summary>
        [BsonElement("Name")]
        [BsonRequired]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Category of product.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
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
