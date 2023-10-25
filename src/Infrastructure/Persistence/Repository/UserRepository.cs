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

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _dbContext.Users
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
