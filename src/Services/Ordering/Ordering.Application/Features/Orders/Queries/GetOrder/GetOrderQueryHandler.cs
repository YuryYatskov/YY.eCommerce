using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrder
{
    /// <summary>
    /// The query handler for getting an order by identifier.
    /// </summary>
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderVm>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="orderRepository"><inheritdoc cref="IOrderRepository" path="/summary"/></param>
        /// <param name="mapper"> Models matching. </param>
        public GetOrderQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// The query handler for getting an order by identifier.
        /// </summary>
        /// <param name="request"><inheritdoc cref="GetOrderQuery" path="/summary"/></param>
        /// <param name="cancellationToken"><inheritdoc cref="CancellationToken" path="/summary"/></param>
        /// <returns> An order. </returns>
        public async Task<OrderVm> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id);
            return _mapper.Map<OrderVm>(order);
        }
    }
}