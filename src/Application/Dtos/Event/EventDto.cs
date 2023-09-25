namespace Application.Dtos.Event
{
    public record EventDto(
        Guid Id,
        string Name,
        string Description,
        string ImageUrl,
        string CategoryName
        );
}
