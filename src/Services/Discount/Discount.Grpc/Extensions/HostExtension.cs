using Npgsql;

namespace Discount.Grpc.Extensions
{
	/// <summary>
	/// The host extension.
	/// </summary>
	public static class HostExtension
	{
		/// <summary>
		/// The migrate database and seed.
		/// </summary>
		/// <typeparam name="TContext"> A call context. </typeparam>
		/// <param name="host"> A host application. </param>
		/// <param name="retry"> The number of attempts to connect to the database. </param>
		/// <returns> A host application. </returns>
		public static IHost MigrateDatabase<TContext>(this IHost host, int retry = 0)
		{
			int retryForAvailability = retry;

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
				command.CommandText = @"
					create table if not exists Coupon (
						Id serial primary key,
						ProductName varchar(24) not null,
						Description text,
						Amount int);";
				command.ExecuteNonQuery();

				command.CommandText = @"
					do
					$do$
					begin
						if not exists (select * from Coupon limit 1) then
							insert into Coupon (ProductName, Description, Amount)
								values ('IPhone X', 'IPhone Discount', 250),
									   ('Samsung 10','Samsung Discount', 300);
					end if;
					end;
					$do$;";
				command.ExecuteNonQuery();

				logger.LogInformation("Migrated postgresql database.");
			}
			catch (NpgsqlException ex)
			{
				logger.LogError(ex, "An error occured while migrating the postgresql database");

				if (retryForAvailability < 50)
				{
					retryForAvailability++;
					Thread.Sleep(2000);
					MigrateDatabase<TContext>(host, retryForAvailability);
				}
			}

			return host;
		}
	}
}
