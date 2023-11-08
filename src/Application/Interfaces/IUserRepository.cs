using Domain.Models.User;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        public Task<int> SaveChangesAsync();
        public Task<User?> GetUserByIdAsync(Guid id);
        public void DeleteUserById(User user);
    }
}
