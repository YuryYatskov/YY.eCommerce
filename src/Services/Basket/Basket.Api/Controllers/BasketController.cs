using Basket.Api.Entities;
using Basket.Api.Repositories;
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

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="basketRepository"> A basket repository. </param>
        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
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
    }
}
