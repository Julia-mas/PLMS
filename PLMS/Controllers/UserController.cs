using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PLMS.API.ApiHelper;
using PLMS.API.Models.ModelsUser;
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
                return ApiResponseHelper.CreateValidationErrorResponse(ModelState);
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
                return ApiResponseHelper.CreateOkResponseWithMessage("User registered successfully", token);
            }
            var errors = string.Join(", ", result.Errors.Select(e => e.Description).ToList());
            return ApiResponseHelper.CreateErrorResponse(errors, StatusCodes.Status400BadRequest);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.CreateValidationErrorResponse(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return ApiResponseHelper.CreateErrorResponse("Invalid login attempt.", StatusCodes.Status401Unauthorized);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateUserTokenAsync(user, "Default", "access_token");
                return ApiResponseHelper.CreateOkResponseWithMessage("User logged in successfully.", token);
            }

            if (result.IsLockedOut)
            {
                return ApiResponseHelper.CreateErrorResponse("User account locked out.", StatusCodes.Status401Unauthorized);
            }

            return ApiResponseHelper.CreateErrorResponse("Invalid login attempt.", StatusCodes.Status401Unauthorized);
        }

        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return ApiResponseHelper.CreateOkResponseWithMessage<string>("User logged out successfully.");
        }

        [HttpPost("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return ApiResponseHelper.CreateErrorResponse("Email not found.", StatusCodes.Status400BadRequest);
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            return ApiResponseHelper.CreateOkResponseWithMessage("Password reset email sent.", resetToken);
        }
    }
}
