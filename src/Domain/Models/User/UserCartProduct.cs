namespace Domain.Models.User
{
    public class UserCartProduct
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
