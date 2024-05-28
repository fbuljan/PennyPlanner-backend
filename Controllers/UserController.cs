using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PennyPlanner.DTOs.User;
using PennyPlanner.Services.Interfaces;

namespace PennyPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService UserService { get; }
        private IAuthService AuthService { get; }

        public UserController(IUserService userService, IAuthService authService)
        {
            UserService = userService;
            AuthService = authService;
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

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserLogin userLogin)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            UserGet? user = await UserService.GetUserByLoginAsync(userLogin.Login);

            if (user == null)
            {
                return Unauthorized(new { success = false, message = "Authentication failed. User not found." });
            }

            var passwordIsValid = AuthService.VerifyPassword(userLogin.Password, user.Password);
            if (!passwordIsValid)
            {
                return Unauthorized(new { success = false, message = "Authentication failed. Incorrect password." });
            }

            var token = AuthService.GenerateJwtToken(user);

            var response = new
            {
                success = true,
                message = "Login successful",
                user = new { user.Id, user.Name, user.Username, user.Email },
                token
            };

            return Ok(response);
        }

        //[Authorize]
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateUser(UserUpdate userUpdate)
        {
            await UserService.UpdateUserAsync(userUpdate);
            var user = await UserService.GetUserAsync(userUpdate.Id);
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

        //[Authorize]
        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await UserService.GetUserAsync(id);
            return Ok(user);
        }

        //[Authorize]
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await UserService.GetUsersAsync();
            return Ok(users);
        }
    }
}
