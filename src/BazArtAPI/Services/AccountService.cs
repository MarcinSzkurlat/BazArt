using Application.Exceptions;
using BazArtAPI.Dtos.User;
using Domain.Models;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BazArtAPI.Services
{
    public class AccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;

        public AccountService(UserManager<User> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) throw new NotFoundException("User not found");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result) throw new BadRequestException("Wrong password");

            return CreateUserDto(user);
        }

        public async Task<UserDto> RegistrationAsync(RegistrationDto registrationDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registrationDto.Email))
            {
                throw new BadRequestException("This email is already taken");
            }

            var user = new User()
            {
                Email = registrationDto.Email,
                StageName = registrationDto.StageName,
                Description = registrationDto.Description,
                UserName = registrationDto.Email.Split('@')[0]
            };

            if (registrationDto.Category != null)
            {
                user.Category = new Category()
                {
                    Id = (int)registrationDto.Category
                };
            }

            var result = await _userManager.CreateAsync(user, registrationDto.Password);

            if (!result.Succeeded) throw new BadRequestException("Some problem with registration");

            await _userManager.AddToRoleAsync(user, "User");

            return CreateUserDto(user);
        }

        private UserDto CreateUserDto(User user)
        {
            return new UserDto(user.Id, user.Email!, user.StageName, _userManager.GetRolesAsync(user).Result.FirstOrDefault()!, _tokenService.CreateToken(user));
        }
    }
}
