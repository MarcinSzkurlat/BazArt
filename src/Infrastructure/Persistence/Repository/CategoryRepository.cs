using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BazArtDbContext _dbContext;

        public CategoryRepository(BazArtDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
