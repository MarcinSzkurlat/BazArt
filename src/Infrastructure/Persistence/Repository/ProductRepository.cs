﻿using Application.Interfaces;
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
    }
}
