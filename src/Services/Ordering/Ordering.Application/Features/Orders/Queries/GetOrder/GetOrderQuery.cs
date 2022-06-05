using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrder
{
    public class GetOrderQuery : IRequest<OrderVm>
    {
        public int Id { get; set; }

        public GetOrderQuery(int id)
        {
            Id = id;
        }

    }
}
