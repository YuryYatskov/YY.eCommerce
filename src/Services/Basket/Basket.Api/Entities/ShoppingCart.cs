namespace Basket.Api.Entities
{
    /// <summary>
    /// A shopping cart.
    /// </summary>
    public class ShoppingCart
    {
        public string UserName { get; set; } = string.Empty;

        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public ShoppingCart()
        {
        }

        public ShoppingCart(string UserName)
        {
            this.UserName = UserName;
        }

        public decimal TotalPrice => Items.Sum(x => x.Quantity * x.Price);
        //{
        //    get
        //    {
        //        decimal totalPrice = 0;
        //        foreach (var item in Items)
        //            totalPrice += item.Quantity * item.Price;
   
        //        return totalPrice;
        //    }
        //}
    }
}
