namespace Application.Dtos.Search
{
    public record SearchResultDto(
        Dictionary<string, SearchCategoryResultDto> Items);
}
