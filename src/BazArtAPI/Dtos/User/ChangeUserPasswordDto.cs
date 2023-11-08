namespace BazArtAPI.Dtos.User
{
    public record ChangeUserPasswordDto(
        string OldPassword,
        string NewPassword);
}
