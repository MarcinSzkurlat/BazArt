using Domain.Models;

namespace BazArtAPI.Dtos.User
{
    public record RegistrationDto(
        string Email,
        string Password,
        string? StageName,
        string? Description,
        Categories? Category);
}
