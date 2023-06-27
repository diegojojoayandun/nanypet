using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NanyPet.Api.Models.Dto.User;
using NanyPet.Api.Repositories.IRepository;
using NanyPet.Api.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoogleAuth.Controllers
{
    [ApiController]
    [AllowAnonymous, Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthController(
            ILogger<AuthController> logger,
            IConfiguration configuration,
            IUserRepository userRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        [HttpGet("signin-google")]
        public IActionResult GoogleAuth()
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleAuthCallback))
            };

            return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("handle-google-callback")]
        public async Task<IActionResult> GoogleAuthCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (authenticateResult.Succeeded)
            {
                //// Authentication successful, handle the user's information.
                //var user = authenticateResult.Principal;

                //// Retrieve user's email
                //var userEmail = user.FindFirstValue(ClaimTypes.Email);

                //// Retrieve user's name
                //var userName = user.FindFirstValue(ClaimTypes.Name);

                //// Redirect or perform further actions.
                //// For example, you can return a success message or redirect to a specific URL.
                //return Ok(new { Email = userEmail, Name = userName });
                ////return RedirectToAction("GetAllUsers", "UserController", new { Email = userEmail, Name = userName });
                var user = authenticateResult.Principal;
                var userEmail = user.FindFirstValue(ClaimTypes.Email);


                var token = GenerateJwtToken(userEmail);

                return Ok(new { token });
            }
            else
            {

                // Authentication failed, handle the error.
                return Unauthorized();
            }


        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto user)
        {
            // Validate user credentials
            if (await IsValidUserCredentials(user.Email, user.Password) == false)
            {
                return Unauthorized(); // Return 401 Unauthorized if credentials are invalid
            }

            // If authentication succeeds, generate and return a JWT token
            var token = GenerateJwtToken(user.Email);

            return Ok(new { token });
        }

        private async Task<bool> IsValidUserCredentials(string email, string password)
        {
            // Perform your user authentication logic here

            if (email == null || password == null)
                return false;

            var UserData = await _userRepository.GetUserByEmail(v => v.Email == email);

            if (UserData != null && UserData.Email == email && PasswordHasher.VerifyPassword(password, UserData.Password))
                return true;

            return false;
        }

        private string GenerateJwtToken(string user)
        {
            // Header

            var _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SECRET_KEY"]));
            var signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            // Claims

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user),
            };

            // Payload

            var payload = new JwtPayload
            (
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(15)
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
