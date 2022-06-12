using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace Catalog.Api.Controllers
{
    /// <summary>
    /// Catalog.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="repository"> Product repository. </param>
        /// <param name="logger"> Logging service. </param>
        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        /// <response code="200"> All products. </response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            _logger.LogInformation("Get all products.");
            var products = await _repository.GetProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Get a product.
        /// </summary>
        /// <param name="id"> Identifier of product. </param>
        /// <response code="200"> A product. </response>
        /// <response code="404"> Product not found. </response>
        [HttpGet("{id:length(24)}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById([FromRoute, Required(AllowEmptyStrings = false)] string id)
        {
            _logger.LogInformation("Get a product by identifier '{id}'.", id);
            var product = await _repository.GetProductAsync(id);

            if (product == null)
            {
                const string message = "Product with identifier '{id}' is not found.";
                _logger.LogWarning(message, id);
                return NotFound(new StatusCodeProblemDetails(StatusCodes.Status404NotFound) { Detail = message.Replace("{id}", id) } );
            }

            return Ok(product);
        }

        /// <summary>
        /// Get products by name.
        /// </summary>
        /// <param name="name"> A name. </param>
        /// <response code="200"> Products. </response>
        [HttpGet("[action]/{name}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByName([FromRoute, Required(AllowEmptyStrings = false)] string name)
        {
            _logger.LogInformation("Get products by name {name}.", name);
            var products = await _repository.GetProductByNameAsync(name);
            return Ok(products);
        }

        /// <summary>
        /// Get products by category.
        /// </summary>
        /// <param name="category"> A category. </param>
        /// <response code="200"> Products. </response>
        [HttpGet("[action]/{category}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory([FromRoute, Required(AllowEmptyStrings = false)] string category)
        {
            _logger.LogInformation("Get products by category {category}.", category);
            var products = await _repository.GetProductByCategoryAsync(category);
            return Ok(products);
        }

        /// <summary>
        /// Create a product.
        /// </summary>
        /// <param name="product"> A product. </param>
        /// <response code="201"> A new product. </response>
        /// <response code="400"> An error occurred while adding a value, validating the model. Bad request. </response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody, Required] Product product)
        {
            _logger.LogInformation("Create a product with name {name}.", product.Name);
            await _repository.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Change a product.
        /// </summary>
        /// <param name="product"> A product. </param>
        /// <response code="204"> A product has been changed. </response>
        /// <response code="400"> An error occurred while changing the value during model validation. Bad request. </response>
        /// <response code="404"> A value with the specified identifier was not found. </response>
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> UpdateProduct([FromBody, Required] Product product)
        {
            _logger.LogInformation("Change a product by id {id}.", product.Id);
            await _repository.UpdateProductAsync(product);
            return NoContent();
        }

        /// <summary>
        /// Delete a product.
        /// </summary>
        /// <param name="id"> Identifier of product. </param>
        /// <response code="204"> A product has been deleted. </response>
        /// <response code="404"> A value with the specified identifier was not found. </response>
        [HttpDelete("{id:length(24)}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProduct([FromRoute, Required(AllowEmptyStrings = false)] string id)
        {
            _logger.LogInformation("Delete a product by id {id}.", id);
            await _repository.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
