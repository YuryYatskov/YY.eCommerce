using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    /// <summary>
    /// The database connection context.
    /// </summary>
    public interface ICatalogContext
    {
        /// <summary>
        /// A product collection.
        /// </summary>
        IMongoCollection<Product> Products { get; }
    }
}