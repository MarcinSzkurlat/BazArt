using Microsoft.AspNetCore.Identity;

namespace Domain.Models.User
{
    public class User : IdentityUser<Guid>
    {
        public string? StageName { get; set; }
        public string? Description { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public UserAddress Address { get; set; }
        public ICollection<Event.Event> CreatedEvents { get; set; } = new List<Event.Event>();
        public ICollection<Product> OwnedProducts { get; set; } = new List<Product>();
        public ICollection<UserCartProduct> UserCartProducts { get; set; } = new List<UserCartProduct>();
        public ICollection<FavoriteUser> FavoriteUsers { get; set; } = new List<FavoriteUser>();
        public ICollection<FavoriteProduct> FavoriteProducts { get; set; } = new List<FavoriteProduct>();
    }
}
