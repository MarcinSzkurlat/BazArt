using Domain.Models.User;

namespace Application.Interfaces
{
    public interface IFavoriteUsersRepository
    {
        public Task SaveChangesAsync();
        public Task<IEnumerable<User>> GetUserFavoritesUsersAsync(Guid id, int pageNumber, int pageSize);
        public Task<int> GetUserFavoritesUsersQuantityAsync(Guid id);
        public Task AddFavoriteUserAsync(Guid userId, Guid favoriteUserId);
        public Task DeleteFavoriteUserAsync(Guid userId, Guid favoriteUserId);
        public Task<bool> UserHasFavoriteUserAsync(Guid userId, Guid favoriteUserId);
        public Task DeleteAllUserFavoritesUsersAsync(Guid userId);
    }
}
