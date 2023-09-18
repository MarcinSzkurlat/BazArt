namespace Domain.Models.User
{
    public class LikeUser
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int LikedUserId { get; set; }
        public User LikedUser { get; set; }
    }
}
