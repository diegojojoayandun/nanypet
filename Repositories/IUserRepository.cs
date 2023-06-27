using NanyPet.Api.Models;
using NanyPet.Api.Repositories.IRepository;
using System.Linq.Expressions;

namespace NanyPet.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmail(Expression<Func<User, bool>>? filter = null, bool tracked = true);
    }
}