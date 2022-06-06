using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    /// <summary>
    /// A discount service.
    /// </summary>
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="discountRepository"> A discount repository. </param>
        /// <param name="logger"> Logging service. </param>
        /// <param name="mapper"> Models matching. </param>
        /// <exception cref="ArgumentNullException"></exception>
        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get a product discount.
        /// </summary>
        /// <param name="request"> A request with a product name.</param>
        /// <param name="context"> Context for a server-side call. </param>
        /// <returns> A product discount. </returns>
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            _logger.LogInformation("{message}. ({productName})", $"Get a product discount by name {request.ProductName}.", request.ProductName);
            var coupon = await _discountRepository.GetDiscountAsync(request.ProductName);
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with product name '{request.ProductName}' is not found."));

            return _mapper.Map<CouponModel>(coupon);
        }

        /// <summary>
        /// Create product discount.
        /// </summary>
        /// <param name="request"> A request with a discount coupon. </param>
        /// <param name="context"> Context for a server-side call. </param>
        /// <returns> A creation result. </returns>
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            _logger.LogInformation("{message}. ({productName})", $"Create a discount for the product '{request.Coupon.ProductName}'.", request.Coupon.ProductName);
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            var couponExist = await _discountRepository.GetDiscountAsync(coupon.ProductName);
            if (couponExist.Id != 0)
                throw new RpcException(new Status(StatusCode.AlreadyExists, $"The discount for the product '{coupon.ProductName}' is already exists."));

            var result = await _discountRepository.CreateDiscountAsync(coupon);
            if (result)
            {
                _logger.LogInformation("The discount for the product '{coupon.ProductName}' with identifier '{coupon.Id}' has been added.", coupon.ProductName, coupon.Id);

                var couponAdded = await _discountRepository.GetDiscountAsync(coupon.ProductName);
                return _mapper.Map<CouponModel>(couponAdded);
            }
            throw new RpcException(new Status(StatusCode.Unknown, $"Unable to create discount for product '{coupon.ProductName}'."));
        }

        /// <summary>
        /// Change product discount.
        /// </summary>
        /// <param name="request"> A request with a discount coupon. </param>
        /// <param name="context"> Context for a server-side call. </param>
        /// <returns> A discount coupon has been changed. </returns>
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            _logger.LogInformation("{message}. ({couponId})", $"Change product discount by id {request.Coupon.Id}.", request.Coupon.Id);
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            var couponExist = await _discountRepository.GetDiscountAsync(request.Coupon.Id);
            if (couponExist is null)
            {
                var messageNotFound = $"The product discount by id '{coupon.Id}' is not found.";
                _logger.LogWarning("{message}. ({couponId})", messageNotFound, coupon.Id);

                throw new RpcException(new Status(StatusCode.NotFound, messageNotFound));
            }

            var result = await _discountRepository.UpdateDiscountAsync(coupon);
            if (result)
            {
                _logger.LogInformation("The discount for the product '{coupon.ProductName}' with identifier '{coupon.Id}' has been changed.", coupon.ProductName, coupon.Id);

                var couponUpdated = await _discountRepository.GetDiscountAsync(coupon.ProductName);
                return _mapper.Map<CouponModel>(couponUpdated);
            }

            var messageNotChange = $"It is impossible to change the discount for the '{coupon.ProductName}' product with identifier '{coupon.Id}'.";
            _logger.LogInformation("{message}. ({productName}, {couponId})", messageNotChange, coupon.ProductName, coupon.Id);

            throw new RpcException(new Status(StatusCode.Unknown, messageNotChange));
        }

        /// <summary>
        /// Delete a product discount.
        /// </summary>
        /// <param name="request"> A request with a product name. </param>
        /// <param name="context"> Context for a server-side call. </param>
        /// <returns> A product discount has been deleted. </returns>
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            _logger.LogInformation("{message}. ({productName})", $"Delete a product discount by name {request.ProductName}.", request.ProductName);
            var result = await _discountRepository.DeleteDiscountAsync(request.ProductName);
            if (result)
                _logger.LogInformation("{message}. ({productName})", $"The discount for the product '{request.ProductName}' has been deleted.", request.ProductName);
            else
                _logger.LogInformation("{message}. ({productName})", $"It is impossible to delete the discount for the '{request.ProductName}' product.", request.ProductName);

            return _mapper.Map<DeleteDiscountResponse>(result);
        }
    }
}