using Marketplace.Infra.Database;
using Marketplace.Models;

namespace Marketplace.Repositories
{
    public interface IProductRepository
    {
        Task<ICollection<Product>> FetchAll();
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

        public Task<Product> CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<Product> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Product>> FetchAll()
        {
            throw new NotImplementedException();
        }

        public Task<Product> FetchById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
