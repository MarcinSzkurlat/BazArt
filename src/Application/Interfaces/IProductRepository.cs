using Domain.Models;

namespace Application.Interfaces
{
    public interface IProductRepository
    {
        public Task<int> SaveChangesAsync();
        public Task<Domain.Models.Product?> GetProductByIdAsync(Guid id);
        public Task<IEnumerable<Domain.Models.Product>> GetProductsByCategoryAsync(Categories category);
        public Task CreateProductAsync(Domain.Models.Product productToCreate);
        public void DeleteProduct(Domain.Models.Product productToDelete);
        public Task<IEnumerable<Product>> GetProductsByCreatedDate(int amount);
    }
}
