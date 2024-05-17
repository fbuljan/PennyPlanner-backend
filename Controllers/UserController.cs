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

        public UserController(IUserService userService)
        {
            UserService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserCreate userCreate)
        {
            if (ModelState.IsValid)
            {
                userCreate.Password = PasswordUtils.HashPassword(userCreate.Password);
                var id = await UserService.CreateUserAsync(userCreate);
                var user = await UserService.GetUserAsync(id);

                var response = new { success = true, user };
                return Ok(response);
            }

            return BadRequest(ModelState);
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> LoginUser(UserLoginModel model)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    User? user = null;

        //    if (!string.IsNullOrWhiteSpace(model.Login))
        //    {
        //        if (RegexUtils.IsValidEmail(model.Login))
        //            user = await context.Users.FirstOrDefaultAsync(u => u.Email == model.Login);
        //        else
        //            user = await context.Users.FirstOrDefaultAsync(u => u.Username == model.Login);
        //    }

        //    if (user == null)
        //    {
        //        return Unauthorized(new { success = false, message = "Authentication failed. User not found." });
        //    }

        //    var passwordIsValid = PasswordUtils.VerifyPassword(model.Password, user.Password);
        //    if (!passwordIsValid)
        //    {
        //        return Unauthorized(new { success = false, message = "Authentication failed. Incorrect password." });
        //    }

        //    var token = GenerateJwtToken(user);

        //    var response = new
        //    {
        //        success = true,
        //        message = "Login successful",
        //        user = new { user.ID, user.Name, user.Username, user.Email },
        //        token
        //    };

        //    return Ok(response);
        //}

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateUser(UserUpdate userUpdate)
        {
            await UserService.UpdateUserAsync(userUpdate);
            var user = UserService.GetUserAsync(userUpdate.Id);
            return Ok(user);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteUser(UserDelete userDelete)
        {
            await UserService.DeleteUserAsync(userDelete);
            return Ok();
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await UserService.GetUserAsync(id);
            return Ok(user);
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await UserService.GetUsersAsync();
            return Ok(users);
        }
    }
}
