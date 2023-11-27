using Domain.Models;

namespace Application.Interfaces
{
    public interface ICartRepository
    {
        public Task SaveChangesAsync();
        public Task<IEnumerable<Product>> GetUserCartAsync(Guid userId);
        public Task<bool> UserHasProductAsync(Guid userId, Guid productId);
        public Task AddProductAsync(Guid userId, Guid productId);
        public Task DeleteProductAsync(Guid userId, Guid productId);
    }
}
