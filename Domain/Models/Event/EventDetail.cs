using Domain.Models.Abstraction;

namespace Domain.Models.Event
{
    public class EventDetail : Address
    {
        public int Id { get; set; }

        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
