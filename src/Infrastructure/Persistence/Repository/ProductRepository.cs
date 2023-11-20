using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly BazArtDbContext _dbContext;

        public ProductRepository(BazArtDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _dbContext.Products
                .Include(x => x.Category)
                .Include(x => x.CreatedBy)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.Category.Name.ToLower() == categoryName.ToLower())
                .ToListAsync();
        }

        public async Task CreateProductAsync(Product productToCreate)
        {
            productToCreate.Category = await _dbContext.Categories.FindAsync(productToCreate.CategoryId);
            productToCreate.CreatedBy = await _dbContext.Users.FindAsync(productToCreate.CreatedById);

            await _dbContext.Products.AddAsync(productToCreate);
        }

        public void DeleteProduct(Product productToDelete)
        {
            _dbContext.Products.Remove(productToDelete);
        }

        public async Task<IEnumerable<Product>> GetProductsByCreatedDate(int amount)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .OrderByDescending(x => x.Created)
                .Take(amount)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByUserIdAsync(Guid id)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.CreatedById == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsBySearchQueryAsync(string searchQuery)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .Where(x => x.Name.ToUpper().Contains(searchQuery.ToUpper()) || x.Description.ToUpper().Contains(searchQuery.ToUpper()))
                .ToListAsync();
        }
    }
}
