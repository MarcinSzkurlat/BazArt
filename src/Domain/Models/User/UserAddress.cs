namespace Domain.Models.User
{
    public class UserAddress
    {
        public int Id { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int? HouseNumber { get; set; }
        public string? PostalCode { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
