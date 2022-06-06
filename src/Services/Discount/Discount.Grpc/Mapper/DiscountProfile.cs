using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mapper
{
    /// <summary>
    /// Models matching profile.
    /// </summary>
    public class DiscountProfile : Profile
    {
        /// <summary>
        /// Initialization.
        /// </summary>
        public DiscountProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
