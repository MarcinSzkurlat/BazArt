using Domain.Models.Abstraction;

namespace Domain.Models.User
{
    public class UserAddress : Address
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
