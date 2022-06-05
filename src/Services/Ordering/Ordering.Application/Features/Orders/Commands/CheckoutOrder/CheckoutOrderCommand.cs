using MediatR;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    /// <summary>
    /// The command to checkout an order.
    /// </summary>
    public class CheckoutOrderCommand : IRequest<int>
    {
        /// <summary>
        /// A user name.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// A total price.
        /// </summary>
        public decimal TotalPrice { get; set; }

        // BillingAddress

        /// <summary>
        /// A first name.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// A last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// An email address.
        /// </summary>
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// An address.
        /// </summary>
        public string AddressLine { get; set; } = string.Empty;

        /// <summary>
        /// A country.
        /// </summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// A state.
        /// </summary>
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// A zip code.
        /// </summary>
        public string ZipCode { get; set; } = string.Empty;

        // Payment

        /// <summary>
        /// A card name.
        /// </summary>
        public string CardName { get; set; } = string.Empty;

        /// <summary>
        /// A card number.
        /// </summary>
        public string CardNumber { get; set; } = string.Empty;

        /// <summary>
        /// An expiration.
        /// </summary>
        public string Expiration { get; set; } = string.Empty;

        /// <summary>
        /// A cw-code.
        /// </summary>
        public string CVV { get; set; } = string.Empty;

        /// <summary>
        /// A payment method.
        /// </summary>
        public int PaymentMethod { get; set; }
    }
}
