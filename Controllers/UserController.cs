using Microsoft.AspNetCore.Mvc;
using ToDoListWebApi.Dtos;
using ToDoListWebApi.Models;
using ToDoListWebApi.Services;

namespace ToDoListWebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IUserService userService, ITokenService tokenService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(LoginUserDto loginUserDto)
        {
            User? user = await userService.LoginAsync(loginUserDto.Email, loginUserDto.Password);
            if (user == null) return Unauthorized();

            return new TokenResponseDto { Token = tokenService.GenerateToken(user) };
        }

        [HttpPost("register")]
        public async Task<ActionResult<TokenResponseDto>> Register(RegisterUserDto registerUserDto)
        {
            User? user = await userService.RegisterAsync(registerUserDto.Name, registerUserDto.Email, registerUserDto.Password);
            if (user == null) return Conflict("Email already exists");

            return new TokenResponseDto { Token = tokenService.GenerateToken(user) };
        }
    }
}