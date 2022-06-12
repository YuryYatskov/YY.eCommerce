namespace Shopping.Aggregator.Models
{
    /// <summary>
    /// A basket.
    /// </summary>
    public class BasketModel
    {
        /// <summary>
        /// A user name.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Items in the shopping cart.
        /// </summary>
        public List<BasketItemExtendedModel> Items { get; set; } = new List<BasketItemExtendedModel>();

        /// <summary>
        /// A total price.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
