using Application.Interfaces;
using Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class FavoriteUsersRepository : IFavoriteUsersRepository
    {
        private readonly BazArtDbContext _dbContext;

        public FavoriteUsersRepository(BazArtDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetUserFavoritesUsersAsync(Guid id, int pageNumber, int pageSize)
        {
            return await _dbContext.FavoriteUsers
                .AsNoTracking()
                .Include(x => x.FavoriteUserObject.Address)
                .Include(x => x.FavoriteUserObject.Category)
                .Where(x => x.UserId == id)
                .Select(x => x.FavoriteUserObject)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetUserFavoritesUsersQuantityAsync(Guid id)
        {
            return await _dbContext.FavoriteUsers
                .CountAsync(x => x.UserId == id);
        }

        public async Task AddFavoriteUserAsync(Guid userId, Guid favoriteUserId)
        {
            await _dbContext.FavoriteUsers
                .AddAsync(new FavoriteUser()
                {
                    UserId = userId,
                    FavoriteUserId = favoriteUserId
                });
        }

        public async Task DeleteFavoriteUserAsync(Guid userId, Guid favoriteUserId)
        {
            var favoriteUser =
                await _dbContext.FavoriteUsers.SingleAsync(
                    x => x.UserId == userId && x.FavoriteUserId == favoriteUserId);

            _dbContext.FavoriteUsers.Remove(favoriteUser);

        }

        public async Task<bool> UserHasFavoriteUserAsync(Guid userId, Guid favoriteUserId)
        {
            return await _dbContext.FavoriteUsers
                .AnyAsync(x => x.UserId == userId && x.FavoriteUserId == favoriteUserId);
        }

        public async Task DeleteAllUserFavoritesUsersAsync(Guid userId)
        {
            var favoriteUsers = await _dbContext.FavoriteUsers
                .Where(x => x.UserId == userId)
                .ToListAsync();

            _dbContext.FavoriteUsers.RemoveRange(favoriteUsers);
        }
    }
}
