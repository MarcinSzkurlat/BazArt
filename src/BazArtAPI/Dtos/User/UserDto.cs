namespace BazArtAPI.Dtos.User
{
    public record UserDto(
        Guid Id,
        string Email,
        string? StageName,
        string Role,
        string Token);
}
