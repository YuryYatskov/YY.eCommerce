using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList
{
    /// <summary>
    /// The query handler for getting a list of orders.
    /// </summary>
    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrderVm>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="orderRepository"><inheritdoc cref="IOrderRepository" path="/summary"/></param>
        /// <param name="mapper"> Models matching. </param>
        public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// The query handler for getting a list of orders.
        /// </summary>
        /// <param name="request"><inheritdoc cref="GetOrdersListQuery" path="/summary"/></param>
        /// <param name="cancellationToken"><inheritdoc cref="CancellationToken" path="/summary"/></param>
        /// <returns> A list of orders. </returns>
        public async Task<List<OrderVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
            return _mapper.Map<List<OrderVm>>(orderList);
        }
    }
}
