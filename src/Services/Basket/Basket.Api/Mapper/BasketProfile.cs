using AutoMapper;
using Basket.Api.Entities;
using EventBus.Messages.Events;

namespace Basket.Api.Mapper
{
    /// <summary>
    /// Models matching profile.
    /// </summary>
    public class BasketProfile : Profile
    {
        /// <summary>
        /// Initialization.
        /// </summary>
        public BasketProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>();
        }
    }
}
