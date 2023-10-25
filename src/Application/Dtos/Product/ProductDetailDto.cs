namespace Application.Dtos.Product
{
    public record ProductDetailDto(
        string Name,
        string Description,
        decimal? Price,
        bool IsForSell,
        int Quantity,
        string ImageUrl,
        string? CreatorStageName,
        string CreatorEmail,
        Guid CreatorId,
        string CategoryName,
        DateTime Created
    );
}
