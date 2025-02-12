using Marketplace.Infra.Database;
using Marketplace.Infra.Exceptions;
using Marketplace.Models;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Repositories
{
    public interface IProductRepository
    {
        ICollection<Product> FetchAll();
        Task<Product> FetchById(int id);
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<Product> DeleteProduct(int id);
    }

    public class EFProductRepository : IProductRepository
    {
        private readonly EFDBAccess _access;
        public EFProductRepository(EFDBAccess access)
        {
            _access = access;
        }

        public ICollection<Product> FetchAll() => _access.Products.OrderByDescending(p => p.CreatedAt).ToList();
        public Task<Product> FetchById(int id) => _access.Products.AsNoTracking().FirstAsync(p => p.Id == id);

        public async Task<Product> CreateProduct(Product product)
        {
            _access.Products.Add(product);
            await _access.SaveChangesAsync();
            return product;
        }

        public async Task<Product> DeleteProduct(int id)
        {
            var product = await _access.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new AppException("Product not found");
            }

            _access.Products.Remove(product);
            await _access.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            _access.Products.Update(product);
            await _access.SaveChangesAsync();
            return product;
        }
    }
}
