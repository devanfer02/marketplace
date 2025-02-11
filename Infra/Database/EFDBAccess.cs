using Marketplace.Models;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Database
{
    public class EFDBAccess : DbContext
    {
        public EFDBAccess(DbContextOptions<EFDBAccess> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
