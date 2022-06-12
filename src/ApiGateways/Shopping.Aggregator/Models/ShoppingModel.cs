namespace Shopping.Aggregator.Models
{
    public class ShoppingModel
    {
        /// <summary>
        /// A user name.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// A basket.
        /// </summary>
        public BasketModel BasketWithProducts { get; set; } = new ();

        /// <summary>
        /// The orders.
        /// </summary>
        public IEnumerable<OrderResponseModel> Orders { get; set; } =  new List<OrderResponseModel>();
    }
}