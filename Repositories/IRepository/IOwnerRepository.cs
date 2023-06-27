using NanyPet.Api.Models;
using System.Linq.Expressions;

namespace NanyPet.Api.Repositories.IRepository
{
    public interface IOwnerRepository : IRepository<Owner>
    {
        Task<Owner?> GetByEmail(Expression<Func<Owner, bool>>? filter = null, bool tracked = true);
    }
}