namespace Application.Dtos.User
{
    public record EditUserDetailsDto(
        string? StageName,
        string? Description,
        string? Country,
        string? City,
        string? Street,
        int? HouseNumber,
        string? PostalCode,
        int? CategoryId);
}
