using System.Net;
using System.Text.Json;

namespace Shopping.Aggregator.Extensions
{
    /// <summary>
    /// A http client extensions.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Read content of http response message.
        /// </summary>
        /// <typeparam name="T"> A data type. </typeparam>
        /// <param name="response"> A http response message. </param>
        /// <returns> Data of type. </returns>
        public static async Task<T?> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<T>();
            else if (response.StatusCode == HttpStatusCode.NotFound)
                return Activator.CreateInstance<T>();

            var content = await response.Content.ReadAsStringAsync();
            var message = JsonSerializer.Serialize(new {
                message = $"Something went wrong calling the API {response.RequestMessage?.RequestUri?.AbsolutePath}.",
                reasonPhrase = response.ReasonPhrase,
                responseApi = JsonSerializer.Deserialize<object>(content)
            });
            throw new HttpRequestException(message, null, response.StatusCode);
        }
    }
}