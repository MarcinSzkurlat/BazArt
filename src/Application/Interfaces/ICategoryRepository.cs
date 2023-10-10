using Domain.Models;

namespace Application.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetCategoriesAsync();
        public Task<Category?> GetCategoryByNameAsync(string name);
    }
}
