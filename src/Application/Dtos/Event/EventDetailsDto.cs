namespace Application.Dtos.Event
{
    public record EventDetailsDto(
        string Name,
        string Description,
        string ImageUrl,
        string CategoryName,
        int CategoryId,
        string? OrganizerName,
        string OrganizerEmail,
        Guid OrganizerId,
        string? Country,
        string? City,
        string? Street,
        int? HouseNumber,
        string? PostalCode,
        DateTime StartingDate,
        DateTime EndingDate,
        DateTime Created
        );
}
