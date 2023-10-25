using Domain.Models.User;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> GetUserByIdAsync(Guid id);
    }
}
