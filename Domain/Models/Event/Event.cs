namespace Domain.Models.Event
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int CreatedById { get; set; }
        public User.User CreatedBy { get; set; }
        public int EventDetailId { get; set; }
        public EventDetail EventDetail { get; set; }
    }
}
