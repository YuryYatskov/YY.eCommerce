using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(
            IOrderRepository orderRepository,
            IMapper mapper,
            IEmailService emailService,
            ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

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

            _logger.LogInformation("{message} {orderId}", $"Order with identifier {orderToUpdate.Id} is successfuly updated.");

            return Unit.Value;
        }
    }
}
