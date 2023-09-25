using Domain.Models;

namespace Application.Dtos.Product
{
    public record EditProductDto(
        string? Name,
        string? Description,
        decimal? Price,
        bool? IsForSell,
        int? Quantity,
        string? ImageUrl,
        Categories Category
        );
}
