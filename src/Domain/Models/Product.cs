namespace Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public bool IsForSell { get; set; }
        public int Quantity { get; set; } = 1;
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid CreatedById { get; set; }
        public User.User CreatedBy { get; set; }
    }
}
