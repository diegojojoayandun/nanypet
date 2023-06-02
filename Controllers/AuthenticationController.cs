using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NanyPet.Api.Models;
using NanyPet.Api.Models.Dto.User;
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
        public IActionResult Login([FromBody] UserDto user)
        {
            // Validate user credentials
            if (!IsValidUserCredentials(user.Email, user.Password))
            {
                return Unauthorized(); // Return 401 Unauthorized if credentials are invalid
            }

            // If authentication succeeds, generate and return a JWT token
            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        private  bool IsValidUserCredentials(string email, string password)
        {
            // Perform your user authentication logic here
            // You can check against a database, an external service, or any other method
            if (email == null || password == null)
                return false;

            var UserEmail = _userRepository.GetUserByEmail(v => v.Email == email);
            //var UserPassword = _userRepository.GetUserById(v => v.Password == password);
            // Example validation: hard-coded credentials
            if (UserEmail != null)
            {
                return true;
            }

            return false;
        }

        private string GenerateJwtToken(UserDto user)
        {
            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            //    var tokenDescriptor = new SecurityTokenDescriptor
            //    {
            //        Subject = new ClaimsIdentity(new[]
            //        {
            //    new Claim(ClaimTypes.Name, "diego jojoa") // Add any additional claims here
            //}),
            //        Expires = DateTime.UtcNow.AddHours(1),
            //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            //        Issuer = _jwtSettings.Issuer,
            //        Audience = _jwtSettings.Audience
            //    };

            //    var token = tokenHandler.CreateToken(tokenDescriptor);
            //    return tokenHandler.WriteToken(token);


            // Header

            var _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            // claims

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
            };

            // payload

            var payload = new JwtPayload
            (
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(2)
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }


    }
}
