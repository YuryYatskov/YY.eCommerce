using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Diagnostics;

namespace Ordering.Api.Extensions
{
    /// <summary>
    /// The host extension.
    /// </summary>
    public static class HostExtensions
    {
		/// <summary>
		/// The migrate database and seed.
		/// </summary>
		/// <typeparam name="TContext"> A call context. </typeparam>
		/// <param name="host"> A host application. </param>
		/// <param name="retry"> The number of attempts to connect to the database. </param>
		/// <returns> A host application. </returns>
		public static IHost MigrateDatabase<TContext>(
			this IHost host,
			Action<TContext, IServiceProvider> seeder,
			int? retry = 0)
				where TContext : DbContext
		{
			int retryForAvailability = retry!.Value;

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
			var logger = services.GetRequiredService<ILogger<TContext>>();
			var context = services.GetService<TContext>();

            Guard.IsNotNull(context, nameof(context));

            try
            {
                logger.LogInformation("Migrating database associated with context {@DbContextName}", typeof(TContext).Name);

                InvokeSeeder(seeder, context, services);

                logger.LogInformation("Migrated database associated with context {@DbContextName}", typeof(TContext).Name);
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {@DbContextName}", typeof(TContext).Name);
                if(retryForAvailability < 50)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    MigrateDatabase<TContext>(host, seeder, retryForAvailability);
                }
            }
            return host;
        }

        private static void InvokeSeeder<TContext>(
            Action<TContext, IServiceProvider> seeder,
            TContext context,
            IServiceProvider services)
                where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
