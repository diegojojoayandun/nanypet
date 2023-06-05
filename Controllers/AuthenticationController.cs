using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NanyPet.Api.Models;
using NanyPet.Api.Models.Dto.User;
using NanyPet.Api.Utils;
using NanyPet.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NanyPet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthenticationController(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
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

            var UserData = await  _userRepository.GetUserByEmail(v => v.Email == email);
            
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
