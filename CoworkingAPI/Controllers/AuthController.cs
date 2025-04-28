using CoworkingAPI.Interfaces;
using CoworkingAPI.Model;
using CoworkingAPI.Requests;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                // Преобразуем DTO в модель User
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    Password = request.Password, // Пароль сохраняем как строку
                    RoleId = request.RoleId     // Устанавливаем роль из запроса
                };

                // Вызываем сервис для регистрации
                var createdUser = await _userService.Register(user);

                // Возвращаем успешный ответ
                return Ok(new
                {
                    Message = "User registered successfully",
                    UserId = createdUser.Id,
                    Username = createdUser.Username,
                    Email = createdUser.Email,
                    RoleId = createdUser.RoleId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _userService.Login(request.Username, request.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
