using Domain.Models;

namespace Application.Interfaces
{
    public interface IProductRepository
    {
        public Task<int> SaveChangesAsync();
        public Task<Product?> GetProductByIdAsync(Guid id);
        public Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName);
        public Task CreateProductAsync(Product productToCreate);
        public void DeleteProduct(Product productToDelete);
        public Task<IEnumerable<Product>> GetProductsByCreatedDate(int amount);
        public Task<IEnumerable<Product>> GetProductsByUserIdAsync(Guid id);
        public Task<IEnumerable<Product>> GetProductsBySearchQueryAsync(string searchQuery);
    }
}
