using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    /// <summary>
    /// The checkout order command handler.
    /// </summary>
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="orderRepository"><inheritdoc cref="IOrderRepository" path="/summary"/></param>
        /// <param name="mapper"> Models matching. </param>
        /// <param name="emailService"><inheritdoc cref="IEmailService" path="/summary"/></param>
        /// <param name="logger"> A logger service. </param>
        public CheckoutOrderCommandHandler(
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

        /// <summary>
        /// The checkout order command handler.
        /// </summary>
        /// <param name="request"><inheritdoc cref="CheckoutOrderCommand" path="/summary"/></param>
        /// <param name="cancellationToken"><inheritdoc cref="CancellationToken" path="/summary"/></param>
        /// <returns> An identifier order. </returns>
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var orderAdded =  await _orderRepository.AddAsync(orderEntity);

            _logger.LogInformation("{message} {orderId}", $"Order {@orderAdded.Id} is successfully created.", orderAdded.Id);

            await SendEmail(orderAdded);

            return orderAdded.Id;
        }

        /// <summary>
        /// Send mail.
        /// </summary>
        /// <param name="order"> An order. </param>
        private async Task SendEmail(Order order)
        {
            var email = new Email() { To = "custumer@example.com", Body = $"Order {order.Id} was created.", Subject = "Order was created" };

            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError("{message} {orderId}", $"Order {order.Id} failed due to an error with the mail service: {ex.Message}", order.Id);
            }
        }
    }
}
