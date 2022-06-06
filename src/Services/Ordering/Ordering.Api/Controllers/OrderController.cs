using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries;
using Ordering.Application.Features.Orders.Queries.GetOrder;
using Ordering.Application.Features.Orders.Queries.GetOrderList;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Get orders by user.
        /// </summary>
        /// <param name="userName"> User name. </param>
        /// <response code="200"> Orders or empty collection. </response>
        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(IEnumerable<OrderVm>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderVm>>> GetOrdersByUserName([FromRoute, Required(AllowEmptyStrings = false)] string userName)
        {
            var query = new GetOrdersListQuery(userName);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        /// <summary>
        /// Get an order by identifier.
        /// </summary>
        /// <param name="id"> An order identifier. </param>
        /// <response code="200"> An order. </response>
        /// <response code="404"> An order not found. </response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(IEnumerable<OrderVm>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderVm>>> GetOrderById([FromRoute, Required] int id)
        {
            var query = new GetOrderQuery(id);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        /// <summary>
        /// Checkout an order.
        /// </summary>
        /// <param name="command"> The command to checkout an order. </param>
        /// <response code="201"> The order has been created. </response>
        /// <response code="400"> An error occurred while adding a value, validating the model. Bad request. </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody, Required] CheckoutOrderCommand command)
        {
            var orderId = await _mediator.Send(command);
            var result = new { id = orderId };
            return CreatedAtAction(nameof(GetOrderById), result, result);
        }

        /// <summary>
        /// Change an order.
        /// </summary>
        /// <param name="command"> The command to update an order. </param>
        /// <response code="204"> An order has been changed. </response>
        /// <response code="400"> An error occurred while changing the value during model validation. Bad request. </response>
        /// <response code="404"> A value with the specified identifier was not found. </response>
        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Delete an order.
        /// </summary>
        /// <param name="id"> An order identifier. </param>
        /// <response code="204"> An order has been deleted. </response>
        /// <response code="404"> A value with the specified identifier was not found. </response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
