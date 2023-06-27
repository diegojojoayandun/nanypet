using NanyPet.Api.Models;
using System.Linq.Expressions;

namespace NanyPet.Api.Repositories.IRepository
{
    public interface IHerderRepository : IRepository<Herder>
    {
        Task<Herder?> GetByEmail(Expression<Func<Herder, bool>>? filter = null, bool tracked = true);
    }
}