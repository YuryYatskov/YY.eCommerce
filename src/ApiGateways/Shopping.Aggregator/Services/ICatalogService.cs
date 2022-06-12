using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    /// <summary>
    /// The catalog service.
    /// </summary>
    public interface ICatalogService
    {
        /// <summary>
        /// Get catalogs.
        /// </summary>
        /// <returns> Catalogs. </returns>
        Task<IEnumerable<CatalogModel>> GetCatalog();

        /// <summary>
        /// Get catalogs by a category.
        /// </summary>
        /// <param name="category"> A category. </param>
        /// <returns> Catalogs. </returns>
        Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category);

        /// <summary>
        /// Get catalog by identifier.
        /// </summary>
        /// <param name="id"> A catalog identifier. </param>
        /// <returns> A catalog. </returns>
        Task<CatalogModel> GetCatalog(string id);
    }
}
