using NanyPet.Api.Models;
using NanyPet.Api.Repositories.IRepository;
using System.Linq.Expressions;

namespace NanyPet.Repositories
{
    public interface IHerderRepository : IRepository<Herder>
    {
        Task<Herder?> GetByEmail(Expression<Func<Herder, bool>>? filter = null, bool tracked = true);
    }
}