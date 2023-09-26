namespace Application.Dtos.Product
{
    public record ProductDetailDto(
        string Name,
        string Description,
        decimal? Price,
        bool IsForSell,
        int Quantity,
        string ImageUrl,
        string CreatorName,
        Guid CreatorId,
        string CategoryName
    );
}
