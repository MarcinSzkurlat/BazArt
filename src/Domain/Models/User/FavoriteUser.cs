namespace Domain.Models.User
{
    public class FavoriteUser
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid FavoriteUserId { get; set; }
        public User FavoriteUserObject { get; set; }
    }
}
