using NanyPet.Api.Models;
using NanyPet.Api.Repositories.IRepository;
using System.Linq.Expressions;

namespace NanyPet.Repositories
{
    public interface IOwnerRepository : IRepository<Owner>
    {
        Task<Owner?> GetByEmail(Expression<Func<Owner, bool>>? filter = null, bool tracked = true);
    }
}