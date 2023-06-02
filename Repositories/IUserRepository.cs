using NanyPet.Api.Models;
using NanyPet.Models;
using System.Linq.Expressions;

namespace NanyPet.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers(Expression<Func<User, bool>>? filter = null);
        Task<User?> GetUserById(Expression<Func<User, bool>>? filter = null, bool tracked = true);
        Task<User?> GetUserByEmail(Expression<Func<User, bool>>? filter = null, bool tracked = true);
        //Task InsertHerder(Herder herder);
        //Task UpdateHerder(Herder herder);
        //Task DeleteHerder(Herder herder);
        Task CreateUser(User user);

    }
}