using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrder
{
    /// <summary>
    /// The query to get order by identifier.
    /// </summary>
    public class GetOrderQuery : IRequest<OrderVm>
    {
        /// <summary>
        /// An order identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="id"> An order identifier. </param>
        public GetOrderQuery(int id)
        {
            Id = id;
        }
    }
}
