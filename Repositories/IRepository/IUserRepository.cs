using NanyPet.Api.Models;
using NanyPet.Api.Models.Dto.Login;
using System.Linq.Expressions;

namespace NanyPet.Api.Repositories.IRepository
{
    public interface IUserRepository 
    {
        bool IsUniqueUser(string userName);
        string GenerateJwtToken(User user);
        Task<LoginResponseDTO> SignIn(LoginRequestDTO loginRequestDTO);
        Task<User> SignUp(RegisterRequestDTO registerRequestDTO);
    }
}