using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NanyPet.Api.Models;
using NanyPet.Api.Models.Dto.Login;
using NanyPet.Api.Repositories.IRepository;
using NanyPet.Api.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NanyPet.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly nanypetContext _context;
        private readonly IConfiguration _configuration;


        public UserRepository(nanypetContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public bool IsUniqueUser(string userName)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == userName.ToLower());
                if (user == null)
                    return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return false;
        }

        public async Task<LoginResponseDTO> SignIn(LoginRequestDTO loginRequestDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequestDTO.EmailUser.ToLower());
            if (user != null && user.Email.ToLower() == loginRequestDTO.EmailUser.ToLower() &&
                PasswordHasher.VerifyPassword(loginRequestDTO.Password, user.Password))
            {
                var token = GenerateJwtToken(user.Email);
                LoginResponseDTO loginResponseDTO = new()
                {
                    Token = token,
                    User = user
                };
                return loginResponseDTO;
            }

            return new LoginResponseDTO()
            {
                Token = "",
                User = null
            };
        }

        public string GenerateJwtToken(string user)
        {
            // Header

            var _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SECRET_KEY"]));
            var signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            // Claims

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user),
                //new Claim(ClaimTypes.Role, user.Rol)
            };

            // Payload

            var payload = new JwtPayload
            (
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(10)
            );
            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<User> SignUp(RegisterRequestDTO registerRequestDTO)
        {
            User user = new()
            {
                Email = registerRequestDTO.Email,
                Password = PasswordHasher.HashPassword(registerRequestDTO.Password),
                FirstName = registerRequestDTO.FirstName,
                LastName = registerRequestDTO.LastName,
                Rol = registerRequestDTO.Rol
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
