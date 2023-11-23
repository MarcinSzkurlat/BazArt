using Application.Interfaces;
using Domain.Models;
using Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class FavoriteProductsRepository : IFavoriteProductsRepository
    {
        private readonly BazArtDbContext _dbContext;

        public FavoriteProductsRepository(BazArtDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetUserFavoritesProductsAsync(Guid id, int pageNumber, int pageSize)
        {
            return await _dbContext.FavoriteProducts
                .AsNoTracking()
                .Include(x => x.FavoriteProductObject.Category)
                .Where(x => x.UserId == id)
                .Select(x => x.FavoriteProductObject)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetUserFavoritesProductsQuantityAsync(Guid id)
        {
            return await _dbContext.FavoriteProducts
                .CountAsync(x => x.UserId == id);
        }

        public async Task AddFavoriteProductAsync(Guid userId, Guid productId)
        {
            await _dbContext.FavoriteProducts
                .AddAsync(new FavoriteProduct()
                {
                    UserId = userId,
                    FavoriteProductId = productId
                });
        }

        public async Task DeleteFavoriteProductAsync(Guid userId, Guid productId)
        {
            var favoriteProduct =
                await _dbContext.FavoriteProducts
                    .SingleAsync(x =>
                    x.UserId == userId && x.FavoriteProductId == productId);

            _dbContext.FavoriteProducts.Remove(favoriteProduct);
        }

        public async Task<bool> UserHasFavoriteProductAsync(Guid userId, Guid productId)
        {
            return await _dbContext.FavoriteProducts
                .AnyAsync(x => x.UserId == userId && x.FavoriteProductId == productId);
        }

        public async Task DeleteAllUserFavoritesProductsAsync(Guid userId)
        {
            var favoriteProducts = await _dbContext.FavoriteProducts
                .Where(x => x.UserId == userId)
                .ToListAsync();

            _dbContext.FavoriteProducts.RemoveRange(favoriteProducts);
        }
    }
}
