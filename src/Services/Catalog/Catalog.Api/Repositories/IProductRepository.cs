using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    /// <summary>
    /// Product repository.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Get all products.
        /// </summary>
        /// <returns> All products. </returns>
        Task<IEnumerable<Product>> GetProductsAsync();

        /// <summary>
        /// Get a product by identifier.
        /// </summary>
        /// <param name="id"> Identifier of product.  </param>
        /// <returns> A product. </returns>
        Task<Product> GetProductAsync(string id);

        /// <summary>
        /// Get products by name.
        /// </summary>
        /// <param name="name"> A name. </param>
        /// <returns> Products. </returns>
        Task<IEnumerable<Product>> GetProductByNameAsync(string name);

        /// <summary>
        /// Get products by category.
        /// </summary>
        /// <param name="categoryName"> A category. </param>
        /// <returns> Products. </returns>
        Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryName);

        /// <summary>
        /// Create a product.
        /// </summary>
        /// <param name="product"> A product. </param>
        Task CreateProductAsync(Product product);

        /// <summary>
        /// Change a product.
        /// </summary>
        /// <param name="product"> A product. </param>
        /// <returns> Change status. </returns>
        Task<bool> UpdateProductAsync(Product product);

        /// <summary>
        /// Delete a product.
        /// </summary>
        /// <param name="id"> Identifier of product.  </param>
        /// <returns> Delete status. </returns>
        Task<bool> DeleteProductAsync(string id);
    }
}