using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    /// <inheritdoc cref="IProductRepository"/>
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="context"> The database connection context. </param>
        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.Find(_ => true).ToListAsync();
        }

        /// <inheritdoc/>
        public Task<Product> GetProductAsync(string id)
        {
            return _context.Products.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Product>> GetProductByNameAsync(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _context.Products.Find(filter).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
            return await _context.Products.Find(filter).ToListAsync();
        }

        /// <inheritdoc/>
        public Task CreateProductAsync(Product product)
        {
            product.Id = string.Empty;
            return _context.Products.InsertOneAsync(product);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateProductAsync(Product product)
        {
            var existProduct = await _context.Products.Find(x => x.Id == product.Id).FirstOrDefaultAsync();
            if (existProduct is null)
                throw new KeyNotFoundException($"The product with identifier '{product.Id}' is not found.");

            var updateResult = await _context.Products.ReplaceOneAsync(x => x.Id == product.Id, product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteProductAsync(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            var existProduct = await _context.Products.Find(filter).FirstOrDefaultAsync();
            if (existProduct is null)
                throw new KeyNotFoundException($"The product with identifier '{id}' is not found.");

            DeleteResult deleteResult = await _context.Products.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
