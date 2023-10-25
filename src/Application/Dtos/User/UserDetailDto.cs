namespace Application.Dtos.User
{
    public record UserDetailDto(
        string Email,
        string? StageName,
        string? Description,
        string? Country,
        string? City,
        string? Street,
        int? HouseNumber,
        string? PostalCode
    );
}
