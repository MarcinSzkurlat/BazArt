using Domain.Models;

namespace Application.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetCategoriesAsync();
    }
}
