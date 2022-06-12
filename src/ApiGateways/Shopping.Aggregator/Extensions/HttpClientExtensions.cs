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

            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Something went wrong calling the API. {response.ReasonPhrase}. {content.Replace("\"", "").TrimEnd()}", null, response.StatusCode);
        }
    }
}