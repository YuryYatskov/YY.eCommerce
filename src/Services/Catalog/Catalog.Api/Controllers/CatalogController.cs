using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

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
            var product = await _repository.GetProductAsync(id);

            if (product == null)
            {
                var message = $"Product with id: {id}, not found.";
                _logger.LogWarning(message, id);
                return NotFound(message);
            }

            return Ok(product);
        }

        /// <summary>
        /// Get products by category.
        /// </summary>
        /// <param name="category"> A category. </param>
        /// <response code="200"> Products. </response>
        [HttpGet("[action]/{category}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory([FromRoute, Required(AllowEmptyStrings = false)] string category)
        {
            var products = await _repository.GetProductByCategoryAsync(category);
            return Ok(products);
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody, Required] Product product)
        {
            await _repository.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);

        }
    }
}
