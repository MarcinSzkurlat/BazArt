namespace Application.Dtos.User
{
    public record UserDetailDto(
        Guid Id,
        string Email,
        string? StageName,
        string? Description,
        string? Country,
        string? City,
        string? Street,
        int? HouseNumber,
        string? PostalCode,
        int? CategoryId,
        string Avatar,
        string BackgroundImage
    );
}
