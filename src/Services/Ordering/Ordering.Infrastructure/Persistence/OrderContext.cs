using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContext : DbContext
    {
        /// <summary>
        /// Orders.
        /// </summary>
        public DbSet<Order> Orders => Set<Order>();

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="options"><inheritdoc cref="DbContextOptions" path="/summary"/></param>
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        /// <inheritdoc/>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = "Tor";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = "Tor";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
