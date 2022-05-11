using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await _orderRepository.GetByIdAsync(request.Id);
            if (orderToDelete is null)
            {
                var message = $"Order with identifier {request.Id} not exist on database.";
                _logger.LogWarning("{message} {orderId}", message, request.Id);
                throw new KeyNotFoundException(message);
            }

            await _orderRepository.DeleteAsync(orderToDelete);

            _logger.LogInformation("{message} {orderId}", $"Order {orderToDelete.Id} is successfully deleted.", orderToDelete.Id);

            return Unit.Value;
        }
    }
}
