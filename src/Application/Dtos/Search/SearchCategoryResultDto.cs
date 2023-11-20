namespace Application.Dtos.Search
{
    public record SearchCategoryResultDto(
        string Name,
        IEnumerable<SearchItemDto> Results);
}
