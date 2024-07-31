using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PLMS.API.Models;
using PLMS.DAL.Entities;

namespace PLMS.API.Controllers
{
    [Route("plms/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                //ToDo: mb use unified response instead of anonimus object
                return BadRequest(new { IsSuccess = false, Message = ModelState.Values.First().Errors.First().ErrorMessage });
            }

            var user = new User 
            { 
                UserName = model.Email, 
                Email = model.Email,
                Name = model.Name, 
                Surname = model.Surname
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { IsSuccess = true, Message = "User registered successfully." });
            }

            return BadRequest(new { IsSuccess = false, Message = string.Join(", ", result.Errors.Select(e => e.Description)) });
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { IsSuccess = false, Message = ModelState.Values.First().Errors.First().ErrorMessage });
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Unauthorized(new { isSuccess = false, message = "Invalid login attempt." });
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)//ToDo: Return tokens
            {
                return Ok(new { IsSuccess = true, Message = "User logged in successfully." });
            }

            if (result.IsLockedOut)
            {
                return Unauthorized(new { IsSuccess = false, Message = "User account locked out." });
            }

            return Unauthorized(new { IsSuccess = false, Message = "Invalid login attempt." });
        }

        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { IsSuccess = true, Message = "User logged out successfully." });
        }

        [HttpPost("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound(new { IsSuccess = false, Message = "Email not found." });
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            return Ok(new { IsSuccess = true, Message = "Password reset email sent.", ResetToken = resetToken });
        }
    }
}
