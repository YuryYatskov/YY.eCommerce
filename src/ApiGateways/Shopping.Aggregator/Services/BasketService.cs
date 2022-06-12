using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using System.Net;

namespace Shopping.Aggregator.Services
{
    /// <inheritdoc  cref="IBasketService"/>
    public class BasketService : IBasketService
    {
        private const string _apiNameService = "/api/v1/Basket";
        private readonly HttpClient _client;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="client"> The http client. </param>
        public BasketService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <inheritdoc/>
        public async Task<BasketModel> GetBasket(string userName)
        {
            var response = await _client.GetAsync($"{_apiNameService}/{userName}");

            var result = await response.ReadContentAs<BasketModel>();
            if (result is null)
                result = new BasketModel();
            return result;
        }
    }
}
