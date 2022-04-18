using Npgsql;

namespace Discount.Api.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Migrating postgresql database.");
                using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectingString"));
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = @"create table if not exists Coupon (
                                            Id serial primary key,
                                            ProductName varchar(24) not null,
                                            Description text,
                                            Amount int);";
                command.ExecuteNonQuery();

                command.CommandText = @"if not exists(select top 1 * from Coupn)
                                        begin
                                            insert into Coupon (ProductName, Description, Amount)
                                            values ('IPhone X', 'IPhone Discount', 250),
                                                   ('Samsung 10','Samsung Discount', 300)
                                        end;";
                logger.LogInformation("Migrated postgresql database.");
            }
            catch (NpgsqlException ex)
            {
                logger.LogError(ex, "An error occured while migrating the postgresql database");
            }
        }
    }
}
