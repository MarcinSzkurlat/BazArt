namespace Domain.Models.User
{
    public class User
    {
        public int Id { get; set; }
        public string? StageNumber { get; set; }
        public string? Description { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public int AddressId { get; set; }
        public UserAddress Address { get; set; }
        public ICollection<Event.Event> CreatedEvents { get; set; } = new List<Event.Event>();
        public ICollection<UserProduct> UserProducts { get; set; } = new List<UserProduct>();
        public ICollection<UserOwnedProduct> UserOwnedProducts { get; set; } = new List<UserOwnedProduct>();
        public ICollection<LikeUser> LikedUsers { get; set; } = new List<LikeUser>();
        public ICollection<LikeProduct> LikedProducts { get; set; } = new List<LikeProduct>();
    }
}
