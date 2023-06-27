using NanyPet.Api.Models;
using System.Linq.Expressions;

namespace NanyPet.Api.Repositories.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmail(Expression<Func<User, bool>>? filter = null, bool tracked = true);
    }
}