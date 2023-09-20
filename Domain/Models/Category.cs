namespace Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<Event.Event> Events { get; set; }
        public ICollection<User.User> Users { get; set; }
    }
}
