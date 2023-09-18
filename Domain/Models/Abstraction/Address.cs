namespace Domain.Models.Abstraction
{
    public abstract class Address
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int? HouseNumber { get; set; }
        public string? PostalCode { get; set; }
    }
}
