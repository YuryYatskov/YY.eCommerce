using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    /// <summary>
    /// The update order command handler.
    /// </summary>
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="orderRepository"><inheritdoc cref="IOrderRepository" path="/summary"/></param>
        /// <param name="mapper"> Models matching. </param>
        /// <param name="logger"> A logger service. </param>
        public UpdateOrderCommandHandler(
            IOrderRepository orderRepository,
            IMapper mapper,
            ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// The update order command handler.
        /// </summary>
        /// <param name="request"><inheritdoc cref="UpdateOrderCommand" path="/summary"/></param>
        /// <param name="cancellationToken"><inheritdoc cref="CancellationToken" path="/summary"/></param>
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id);
            if (orderToUpdate is null)
            {
                var message = $"Order with identifier {request.Id} not exist on database.";
                _logger.LogWarning("{message} {orderId}", message, request.Id);
                throw new KeyNotFoundException(message);
            }

            _mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));

            await _orderRepository.UpdateAsync(orderToUpdate);

            _logger.LogInformation("{message} {orderId}", $"Order with identifier {orderToUpdate.Id} is successfuly updated.", orderToUpdate.Id);

            return Unit.Value;
        }
    }
}