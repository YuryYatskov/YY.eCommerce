using Discount.Grpc.Protos;

namespace Basket.Api.Services
{
    /// <summary>
    /// A client of discount service.
    /// </summary>
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountServiceClient;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="discountServiceClient"> A client of discount service. </param>
        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountServiceClient)
        {
            _discountServiceClient = discountServiceClient ?? throw new ArgumentNullException(nameof(discountServiceClient));
        }

        /// <summary>
        /// Get a product discount.
        /// </summary>
        /// <param name="productName"> A product name.</param>
        /// <returns> A product discount. </returns>
        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest { ProductName = productName };
            return await _discountServiceClient.GetDiscountAsync(discountRequest);
        }
    }
}
