using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order() {
                    UserName = "Tor",
                    TotalPrice = 350,

                    FirstName = "Tor",
                    LastName = "Fracosta",
                    EmailAddress = "tor@example.com",
                    AddressLine = "C. de Villarroel, 48",
                    Country = "Barcelona",
                    State = "Catalonia",
                    ZipCode = "08",

                    CardName = "TOR",
                    CardNumber = "1111 2222 3333 4444",
                    Expiration = "05/2030",
                    CVV = "123",
                    PaymentMethod = 1
                }
            };
        }
    }
}
