using BazArtAPI.Dtos.User;
using BazArtAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BazArtAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> LoginAsync([FromBody] LoginDto loginDto)
        {
            var userDto = await _accountService.LoginAsync(loginDto);
            
            return Ok(userDto);
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<ActionResult<UserDto>> RegistrationAsync([FromBody] RegistrationDto registrationDto)
        {
            var userDto = await _accountService.RegistrationAsync(registrationDto);

            return Ok(userDto);
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUserAsync()
        {
            var userDto = await _accountService.GetCurrentUserAsync(User);

            return Ok(userDto);
        }
    }
}
