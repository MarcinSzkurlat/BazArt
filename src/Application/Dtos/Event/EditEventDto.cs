using Domain.Models;

namespace Application.Dtos.Event
{
    public record EditEventDto(
        string? Name,
        string? Description,
        string? ImageUrl,
        Categories Category,
        string? Country,
        string? City,
        string? Street,
        int? HouseNumber,
        string? PostalCode,
        DateTime? StartingDate,
        DateTime? EndingDate
        );
}
