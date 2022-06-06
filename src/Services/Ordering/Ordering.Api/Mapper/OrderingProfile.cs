using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.Api.Mapper
{
    /// <summary>
    /// Models matching profile.
    /// </summary>
    public class OrderingProfile : Profile
    {
        /// <summary>
        /// Initialization.
        /// </summary>
        public OrderingProfile()
        {
            CreateMap<BasketCheckoutEvent, CheckoutOrderCommand>();
        }
    }
}
