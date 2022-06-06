using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    /// <inheritdoc cref="ICatalogContext"/>
    public class CatalogContext : ICatalogContext
    {
        /// <inheritdoc/>
        public IMongoCollection<Product> Products { get; }

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="configuration"> A database connection configuration. </param>
        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            CatalogContextSeed.SeedData(Products);
        }
    }
}
