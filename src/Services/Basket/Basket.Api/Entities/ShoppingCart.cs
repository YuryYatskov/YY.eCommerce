namespace Basket.Api.Entities
{
    /// <summary>
    /// A shopping cart.
    /// </summary>
    public class ShoppingCart
    {
        /// <summary>
        /// A user name.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Items in the shopping cart.
        /// </summary>
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        /// <summary>
        /// Initialization.
        /// </summary>
        public ShoppingCart()
        {
        }

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="UserName"> A user name. </param>
        public ShoppingCart(string UserName)
        {
            this.UserName = UserName;
        }

        /// <summary>
        /// A total price.
        /// </summary>
        public decimal TotalPrice => Items.Sum(x => x.Quantity * x.Price);
    }
}
