using Microsoft.AspNetCore.Mvc;
using ToDoListWebApi.Services;

namespace ToDoListWebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IUserService userService, ITokenService tokenService) : ControllerBase
    {
        public 
    }
}