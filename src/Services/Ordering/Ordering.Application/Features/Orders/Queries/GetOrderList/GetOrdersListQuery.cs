using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList
{
    /// <summary>
    /// Query for a list of orders.
    /// </summary>
    public class GetOrdersListQuery : IRequest<List<OrderVm>>
    {
        /// <summary>
        /// A user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="userName"> A user name. </param>
        public GetOrdersListQuery(string userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}
