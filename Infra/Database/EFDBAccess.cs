using Marketplace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Marketplace.Infra.Database
{
    public class EFDBAccess : DbContext
    {
        public EFDBAccess(DbContextOptions<EFDBAccess> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var dateTimeOffsetConverter = new ValueConverter<DateTimeOffset, DateTimeOffset>(
                d => d.ToUniversalTime(),
                d => d.ToUniversalTime()
            );

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTimeOffset))
                    {
                        property.SetValueConverter(dateTimeOffsetConverter);
                    }
                }
            }
        }
    }
}
