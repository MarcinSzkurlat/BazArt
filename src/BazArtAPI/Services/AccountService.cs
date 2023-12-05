using System.Security.Claims;
using Application.Exceptions;
using BazArtAPI.Dtos.User;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BazArtAPI.Services
{
    public class AccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _config;

        public AccountService(UserManager<User> userManager, TokenService tokenService, IConfiguration config)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _config = config;
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                throw new ValidationException(new() { { "Email", new[] { "Email not found" } } });

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
                throw new ValidationException(new() { { "Password", new[] { "Wrong password" } } });

            return CreateUserDto(user);
        }

        public async Task<UserDto> RegistrationAsync(RegistrationDto registrationDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registrationDto.Email))
                throw new ValidationException(new() { { "Email", new[] { "Email is already taken" } } });

            var user = new User()
            {
                Email = registrationDto.Email,
                StageName = registrationDto.StageName,
                Description = registrationDto.Description,
                UserName = registrationDto.Email.Split('@')[0],
                Avatar = _config["Images:PlaceHolders:User:Avatar"],
                BackgroundImage = _config["Images:PlaceHolders:User:BackgroundImage"]
            };

            if (registrationDto.Category != null)
            {
                user.CategoryId = (int)registrationDto.Category;
            }

            var result = await _userManager.CreateAsync(user, registrationDto.Password);

            if (!result.Succeeded)
                throw new ValidationException(new()
                    { { "Server", new[] { "Some problems with registration", "Please try again later!" } } });

            await _userManager.AddToRoleAsync(user, "User");

            return CreateUserDto(user);
        }

        public async Task<UserDto> GetCurrentUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.FindByEmailAsync(claimsPrincipal.FindFirstValue(ClaimTypes.Email)!);

            return CreateUserDto(user!);
        }

        public async Task<UserDto> ChangeCurrentUserPasswordAsync(ClaimsPrincipal claimsPrincipal, ChangeUserPasswordDto changeUserPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(claimsPrincipal.FindFirstValue((ClaimTypes.Email))!);

            if (user == null)
                throw new ValidationException(new() { { "User", new[] { "User not found" } } });

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, changeUserPasswordDto.OldPassword);

            if (!isPasswordCorrect)
                throw new ValidationException(new() { { "Password", new[] { "Wrong old password" } } });

            var result = await _userManager.ChangePasswordAsync(user, changeUserPasswordDto.OldPassword, changeUserPasswordDto.NewPassword);

            if (!result.Succeeded)
                throw new ValidationException(new()
                {
                    { "Password", result.Errors.Select(error => error.Description).ToArray() }
                });

            return CreateUserDto(user);
        }

        private UserDto CreateUserDto(User user)
        {
            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault()!;
            return new UserDto(user.Id, user.Email!, user.StageName, role, _tokenService.CreateToken(user, role));
        }
    }
}
