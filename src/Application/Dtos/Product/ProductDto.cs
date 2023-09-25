namespace Application.Dtos.Product
{
    public record ProductDto(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        string ImageUrl,
        string CategoryName
        );
}
