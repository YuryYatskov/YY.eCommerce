using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;

namespace Shopping.Aggregator.Controllers
{
    /// <summary>
    /// The shopping.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(ICatalogService catalogSwrvice, IBasketService basketSwrvice, IOrderService orderSwrvice)
        {
            _catalogService = catalogSwrvice ?? throw new ArgumentNullException(nameof(catalogSwrvice));
            _basketService = basketSwrvice ?? throw new ArgumentNullException(nameof(basketSwrvice));
            _orderService = orderSwrvice ?? throw new ArgumentNullException(nameof(orderSwrvice));
        }

        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(ShoppingModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            var basket = await _basketService.GetBasket(userName);

            foreach (var item in basket.Items)
            {
                var product = await _catalogService.GetCatalog(item.ProductId);

                // Set additional product fields.
                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }

            var orders = await _orderService.GetOrdersByUserName(userName);

            var shoppingModel = new ShoppingModel
            {
                UserName = userName,
                BasketWithProducts = basket,
                Orders = orders
            };

            return Ok(shoppingModel);
        }
    }
}
