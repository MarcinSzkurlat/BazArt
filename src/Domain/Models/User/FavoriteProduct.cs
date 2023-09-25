namespace Domain.Models.User
{
    public class FavoriteProduct
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid FavoriteProductId { get; set; }
        public Product FavoriteProductObject { get; set; }
    }
}
