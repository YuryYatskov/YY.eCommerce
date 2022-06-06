using MediatR;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    /// <summary>
    /// The command to delete an order.
    /// </summary>
    public class DeleteOrderCommand : IRequest
    {
        /// <summary>
        /// An order identifier.
        /// </summary>
        public int Id { get; set; }
    }
}
