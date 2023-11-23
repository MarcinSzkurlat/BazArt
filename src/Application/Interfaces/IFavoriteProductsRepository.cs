using Domain.Models;

namespace Application.Interfaces
{
    public interface IFavoriteProductsRepository
    {
        public Task SaveChangesAsync();
        public Task<IEnumerable<Product>> GetUserFavoritesProductsAsync(Guid id, int pageNumber, int pageSize);
        public Task<int> GetUserFavoritesProductsQuantityAsync(Guid id);
        public Task AddFavoriteProductAsync(Guid userId, Guid productId);
        public Task DeleteFavoriteProductAsync(Guid userId, Guid productId);
        public Task<bool> UserHasFavoriteProductAsync(Guid userId, Guid productId);
        public Task DeleteAllUserFavoritesProductsAsync(Guid  userId);
    }
}
