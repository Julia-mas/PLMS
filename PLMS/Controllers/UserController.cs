using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PLMS.API.ApiHelper;
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
                var errorMessage = string.Join(", ", ModelState.Values.First().Errors.First().ErrorMessage);
                return ApiResponseHelper.CreateErrorResponse(errorMessage, 400);
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
                var token = await _userManager.GenerateUserTokenAsync(user, "Default", "access_token");
                return ApiResponseHelper.CreateResponse("User registered successfully", 200, token);
            }
            var errors = string.Join(", ", result.Errors.Select(e => e.Description).ToList());
            return ApiResponseHelper.CreateErrorResponse(errors, 400);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(", ", ModelState.Values.First().Errors.First().ErrorMessage);
                return ApiResponseHelper.CreateErrorResponse(errorMessage, 400);
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return ApiResponseHelper.CreateErrorResponse("Invalid login attempt.", 401);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateUserTokenAsync(user, "Default", "access_token");
                return ApiResponseHelper.CreateResponse("User logged in successfully.", 200, token);
            }

            if (result.IsLockedOut)
            {
                return ApiResponseHelper.CreateErrorResponse("User account locked out.", 401);
            }

            return ApiResponseHelper.CreateErrorResponse("Invalid login attempt.", 401);
        }

        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return ApiResponseHelper.CreateResponse<string>("User logged out successfully.");
        }

        [HttpPost("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return ApiResponseHelper.CreateErrorResponse("Email not found.", 400);
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            return ApiResponseHelper.CreateResponse("Password reset email sent.", 200, resetToken);
        }
    }
}
