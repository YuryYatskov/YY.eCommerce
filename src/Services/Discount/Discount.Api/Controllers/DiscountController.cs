﻿using Discount.Api.Entities;
using Discount.Api.Repositories;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace Discount.Api.Controllers
{
    /// <summary>
    /// Discount.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountController> _logger;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="discountRepository"> A discount repository. </param>
        /// <param name="logger"> Logging service. </param>
        public DiscountController(IDiscountRepository discountRepository, ILogger<DiscountController> logger)
        {
            _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get a product discount.
        /// </summary>
        /// <param name="productName"> A product name. </param>
        /// <response code="200"> A product discount. </response>
        [HttpGet("{productName}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        public async Task<ActionResult<Coupon>> GetDiscount([FromRoute, Required(AllowEmptyStrings = false)] string productName)
        {
            _logger.LogInformation("Get a product discount by name {productName}.", productName);
            var coupon = await _discountRepository.GetDiscountAsync(productName);
            return Ok(coupon);
        }

        /// <summary>
        /// Create product discount.
        /// </summary>
        /// <param name="coupon"> A discount coupon. </param>
        /// <response code="201"> A creation result. </response>
        /// <response code="400"> An error occurred while adding a value, validating the model. Bad request. </response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Coupon>> CreateProduct([FromBody, Required] Coupon coupon)
        {
            _logger.LogInformation("Create a discount for the product '{productName}'.", coupon.ProductName);

            var couponExist = await _discountRepository.GetDiscountAsync(coupon.ProductName);
            if (couponExist.Id != 0)
                return BadRequest($"The discount for the product '{coupon.ProductName}' is already exists.");

            var result = await _discountRepository.CreateDiscountAsync(coupon);
            if (result)
            {
                var couponAdded = await _discountRepository.GetDiscountAsync(coupon.ProductName);
                return CreatedAtAction(nameof(GetDiscount),
                    new { productName = coupon.ProductName },
                    couponAdded);
            }
            return BadRequest();
        }

        /// <summary>
        /// Change product discount.
        /// </summary>
        /// <param name="coupon"> A discount coupon. </param>
        /// <response code="204"> A discount coupon has been changed. </response>
        /// <response code="400"> An error occurred while changing the value during model validation. Bad request. </response>
        /// <response code="404"> A value with the specified identifier was not found. </response>
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Coupon>> UpdateProduct([FromBody, Required] Coupon coupon)
        {
            _logger.LogInformation("Change product discount by id {id}.", coupon.Id);

            var couponExist = await _discountRepository.GetDiscountAsync(coupon.Id);
            if (couponExist is null)
            {
                const string message = "The product discount by id '{coupon.Id}' is not found.";
                _logger.LogWarning(message, coupon.Id);

                return NotFound(new StatusCodeProblemDetails(StatusCodes.Status404NotFound)
                { Detail = message.Replace("{coupon.Id}", coupon.Id.ToString()) });
            }

            var result = await _discountRepository.UpdateDiscountAsync(coupon);
            if (result)
            {
                _logger.LogInformation("The discount for the product '{coupon.ProductName}' with identifier '{coupon.Id}' has been changed.", coupon.ProductName, coupon.Id);
                return NoContent();
            }

            const string messageNotChange = "It is impossible to change the discount for the '{coupon.ProductName}' product with identifier '{coupon.Id}'.";
            _logger.LogInformation(messageNotChange, coupon.ProductName, coupon.Id);

            return BadRequest(new StatusCodeProblemDetails(StatusCodes.Status400BadRequest)
                    { Detail = messageNotChange
                                .Replace("{coupon.ProductName}", coupon.ProductName)
                                .Replace("{coupon.Id}", coupon.Id.ToString()) });
        }

        /// <summary>
        /// Delete a product discount.
        /// </summary>
        /// <param name="productName"> A product name. </param>
        /// <response code="204"> A product discount has been deleted. </response>
        [HttpDelete("{productName}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteDiscount([FromRoute, Required(AllowEmptyStrings = false)] string productName)
        {
            _logger.LogInformation("Delete a product discount by name {productName}.", productName);
            await _discountRepository.DeleteDiscountAsync(productName);
            return NoContent();
        }
    }
}
