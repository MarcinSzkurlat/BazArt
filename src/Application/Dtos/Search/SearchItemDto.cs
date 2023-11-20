namespace Application.Dtos.Search
{
    public record SearchItemDto(
        Guid Id,
        string Title,
        string Description,
        string? Image,
        string Link);
}
