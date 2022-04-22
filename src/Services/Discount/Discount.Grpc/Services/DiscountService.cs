using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountRepository.GetDiscountAsync(request.ProductName);
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with product name '{request.ProductName}' is not found."));

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Create a discount for the product '{productName}'.", request.Coupon.ProductName);
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            var couponExist = await _discountRepository.GetDiscountAsync(coupon.ProductName);
            if (couponExist.Id != 0)
                throw new RpcException(new Status(StatusCode.AlreadyExists, $"The discount for the product '{coupon.ProductName}' is already exists."));

            var result = await _discountRepository.CreateDiscountAsync(coupon);
            if (result)
            {
                var couponAdded = await _discountRepository.GetDiscountAsync(coupon.ProductName);
                var couponModel = _mapper.Map<CouponModel>(couponAdded);
                return couponModel;
            }
            throw new RpcException(new Status(StatusCode.Unknown, $"Unable to create discount for product '{coupon.ProductName}'."));
        }

        public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            return base.UpdateDiscount(request, context);
        }

        public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            return base.DeleteDiscount(request, context);
        }
    }
}
