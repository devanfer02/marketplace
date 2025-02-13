using Marketplace.Infra.Database;
using Marketplace.Models;
using Marketplace.Infra.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Marketplace.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> GetUserByID(string id);
        public Task<User?> GetUserByEmail(string email);
        public Task<User> CreateUser(User user);
        public Task UpdateUser(User user);  
    }

    public class EFUserRepository: IUserRepository
    {
        private readonly EFDBAccess _dbContext;
        public EFUserRepository(EFDBAccess dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateUser(User user)
        {
            try
            {
                user.Id = Guid.NewGuid().ToString();
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
                {
                    throw new AppException("Email is already in use");
                }
            }

            return user;
        }

        public async Task<User?> GetUserByID(string id) => 
             await _dbContext.Users
                .AsNoTracking()
                .Include("Products")
                .FirstAsync(u => u.Id == id);

        public async Task<User?> GetUserByEmail(string email) => 
            await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

        public async Task UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
