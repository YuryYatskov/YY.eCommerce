using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    /// <inheritdoc cref="ICatalogService"/>
    public class CatalogService : ICatalogService
    {
        private const string _apiNameService = "/api/v1/Catalog";
        private readonly HttpClient _client;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="client"> The http client. </param>
        public CatalogService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            var response = await _client.GetAsync(_apiNameService);
            var result = await response.ReadContentAs<List<CatalogModel>>();
            if(result is null)
                return new List<CatalogModel>();
            return result;
        }

        /// <inheritdoc/>
        public async Task<CatalogModel> GetCatalog(string id)
        {
            var response = await _client.GetAsync($"{_apiNameService}/{id}");
            var result = await response.ReadContentAs<CatalogModel>();
            if (result is null)
                return new CatalogModel();
            return result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
        {
            var response = await _client.GetAsync($"{_apiNameService}/GetProductByCategory/{category}");
            var result = await response.ReadContentAs<List<CatalogModel>>();
            if (result is null)
                return new List<CatalogModel>();
            return result;
         }
    }
}
