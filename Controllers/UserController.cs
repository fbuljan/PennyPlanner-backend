using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PennyPlanner.DTOs.User;
using PennyPlanner.Models;
using PennyPlanner.Services.Interfaces;
using PennyPlanner.Utils;

namespace PennyPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService UserService { get; }
        private IConfiguration Configuration { get; }

        public UserController(IUserService userService, IConfiguration configuration)
        {
            UserService = userService;
            Configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserCreate userCreate)
        {
            if (ModelState.IsValid)
            {
                var id = await UserService.CreateUserAsync(userCreate);
                var user = await UserService.GetUserAsync(id);

                var response = new { success = true, user };
                return Ok(response);
            }

            return BadRequest(ModelState);
        }


        //todo extract to user service?
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserLogin userLogin)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            UserGet? user = null;

            if (!string.IsNullOrWhiteSpace(userLogin.Login))
            {
                var users = await UserService.GetUsersAsync();

                if (RegexUtils.IsValidEmail(userLogin.Login))
                    user = users.FirstOrDefault(u => u.Email == userLogin.Login);
                else
                    user = users.FirstOrDefault(u => u.Username == userLogin.Login);
            }

            if (user == null)
            {
                return Unauthorized(new { success = false, message = "Authentication failed. User not found." });
            }

            var passwordIsValid = PasswordUtils.VerifyPassword(userLogin.Password, user.Password);
            if (!passwordIsValid)
            {
                return Unauthorized(new { success = false, message = "Authentication failed. Incorrect password." });
            }

            var token = AuthUtils.GenerateJwtToken(user, Configuration);

            var response = new
            {
                success = true,
                message = "Login successful",
                user = new { user.Id, user.Name, user.Username, user.Email },
                token
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateUser(UserUpdate userUpdate)
        {
            await UserService.UpdateUserAsync(userUpdate);
            var user = UserService.GetUserAsync(userUpdate.Id);
            return Ok(user);
        }

        [Authorize]
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteUser(UserDelete userDelete)
        {
            await UserService.DeleteUserAsync(userDelete);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await UserService.GetUserAsync(id);
            return Ok(user);
        }

        [Authorize]
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await UserService.GetUsersAsync();
            return Ok(users);
        }
    }
}
