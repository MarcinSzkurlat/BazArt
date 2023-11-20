using Application.Interfaces;
using Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BazArtDbContext _dbContext;

        public UserRepository(BazArtDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _dbContext.Users
                .Include(x => x.Address)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void DeleteUserById(User user)
        {
            _dbContext.Users.Remove(user);
        }

        public async Task<IEnumerable<User>> GetUsersBySearchQueryAsync(string searchQuery)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .Where(x => x.StageName.ToUpper().Contains(searchQuery.ToUpper()) || x.Description.ToUpper().Contains(searchQuery.ToUpper()))
                .ToListAsync();
        }
    }
}
