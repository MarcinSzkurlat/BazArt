using Application.Interfaces;
using Domain.Models;
using Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly BazArtDbContext _dbContext;

        public CartRepository(BazArtDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetUserCartAsync(Guid userId)
        {
            return await _dbContext.UsersCartProducts
                .AsNoTracking()
                .Include(x => x.Product.Category)
                .Where(x => x.UserId == userId)
                .Select(x => x.Product)
                .ToListAsync();
        }

        public async Task<bool> UserHasProductAsync(Guid userId, Guid productId)
        {
            return await _dbContext.UsersCartProducts
                .AnyAsync(x => x.UserId == userId && x.ProductId == productId);
        }

        public async Task AddProductAsync(Guid userId, Guid productId)
        {
            await _dbContext.UsersCartProducts
                .AddAsync(new UserCartProduct
                {
                    UserId = userId,
                    ProductId = productId
                });
        }

        public async Task DeleteProductAsync(Guid userId, Guid productId)
        {
            var product = await _dbContext.UsersCartProducts
                .SingleAsync(x => x.ProductId == productId && x.UserId == userId);

            _dbContext.UsersCartProducts.Remove(product);
        }
    }
}
