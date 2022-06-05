using AutoMapper;
using Basket.Api.Entities;
using Basket.Api.Repositories;
using Basket.Api.Services;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace Basket.Api.Controllers
{
    /// <summary>
    /// Basket.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="basketRepository"> A basket repository. </param>
        /// <param name="discountGrpcService"> A discount service. </param>
        /// <param name="mapper"> A mapper models. </param>
        /// <param name="publishEndpoint"><inheritdoc cref="IPublishEndpoint" path="/summary"/></param>
        public BasketController(
            IBasketRepository basketRepository,
            DiscountGrpcService discountGrpcService,
            IMapper mapper,
            IPublishEndpoint publishEndpoint)
        {
            _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentException(nameof(publishEndpoint));
        }

        /// <summary>
        /// Get a basket.
        /// </summary>
        /// <param name="userName"> A user name. </param>
        /// <response code="200"> A basket. </response>
        [HttpGet("{userName}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket([FromRoute, Required(AllowEmptyStrings = false)] string userName)
        {
            var basket = await _basketRepository.GetBasketAsync(userName);
            return Ok(basket);
        }

        /// <summary>
        /// Add or update a basket.
        /// </summary>
        /// <param name="basket"> A basket. </param>
        /// <response code="200"> A basket. </response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody, Required] ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            var basketUpdated = await _basketRepository.UpdateBasketAsync(basket);
            return Ok(basketUpdated);
        }

        /// <summary>
        /// Delete a basket.
        /// </summary>
        /// <param name="userName"> A user name. </param>
        /// <response code="204"> A basket has been deleted. </response>
        [HttpDelete("{userName}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute, Required(AllowEmptyStrings = false)] string userName)
        {
            await _basketRepository.DeleteBasketAsync(userName);
            return NoContent();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // Get an existing basket with a total price.
            var basket = await _basketRepository.GetBasketAsync(basketCheckout.UserName);
            if(basket == null)
                return NotFound($"No basket found for user named {basketCheckout.UserName}.");

            // Send a checkout event to rabbitmq.
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);

            // Remove the basket.
            await _basketRepository.DeleteBasketAsync(basketCheckout.UserName);

            return Accepted();
        }
    }
}
