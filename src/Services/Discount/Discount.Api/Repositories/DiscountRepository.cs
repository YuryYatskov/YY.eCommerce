using Dapper;
using Discount.Api.Entities;
using Npgsql;

namespace Discount.Api.Repositories
{
    /// <inheritdoc cref="IDiscountRepository"/>
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="configuration"> A database connection configuration. </param>
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// A connecting string.
        /// </summary>
        private string ConnectingString() => _configuration.GetValue<string>("DatabaseSettings:ConnectingString");

        /// <inheritdoc/>
        public async Task<Coupon> GetDiscountAsync(string productName)
        {
            using var connection = new NpgsqlConnection(ConnectingString());

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                "select * from Coupon where ProductName = @ProductName", new { ProductName = productName });

            if(coupon is null)
                return new Coupon { ProductName = productName, Amount = 0, Description = "No discount." };

            return coupon;
        }

        /// <inheritdoc/>
        public async Task<Coupon?> GetDiscountAsync(int id)
        {
            using var connection = new NpgsqlConnection(ConnectingString());

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                "select * from Coupon where Id = @Id", new { Id = id });

            return coupon;
        }

        /// <inheritdoc/>
        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(ConnectingString());

            var affected = await connection.ExecuteAsync(
                @"insert into Coupon (ProductName, Description, Amount)
                    values (@ProductName, @Description, @Amount)",
                new {
                    coupon.ProductName,
                    coupon.Description,
                    coupon.Amount
                });

            return affected != 0;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateDiscountAsync(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(ConnectingString());

            var affected = await connection.ExecuteAsync(
                @"update Coupon
                    set ProductName = @ProductName,
                        Description = @Description,
                        Amount = @Amount
                    where Id = @Id",
                new {
                    coupon.ProductName,
                    coupon.Description,
                    coupon.Amount,
                    id = coupon.Id
                });

            return affected != 0;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteDiscountAsync(string productName)
        {
            using var connection = new NpgsqlConnection(ConnectingString());

            var affected = await connection.ExecuteAsync(
                "delete from Coupon where ProductName = @ProductName",
                new { ProductName = productName });

            return affected != 0;
        }
    }
}
