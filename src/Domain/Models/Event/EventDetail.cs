namespace Domain.Models.Event
{
    public class EventDetail
    {
        public int Id { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int? HouseNumber { get; set; }
        public string? PostalCode { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }

        public Guid EventId { get; set; }
        public Event Event { get; set; }
    }
}
