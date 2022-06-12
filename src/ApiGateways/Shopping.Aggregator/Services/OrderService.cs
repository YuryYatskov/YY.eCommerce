using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    /// <inheritdoc cref="IOrderService"/>
    public class OrderService : IOrderService
    {
        private const string _apiNameService = "/api/v1/Order";
        private readonly HttpClient _client;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="client"> The http client. </param>
        public OrderService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
        {
            var response = await _client.GetAsync($"{_apiNameService}/{userName}");
            var result = await response.ReadContentAs<IEnumerable<OrderResponseModel>>();
            if (result is null)
                return new List<OrderResponseModel>();
            return result;
        }
    }
}
