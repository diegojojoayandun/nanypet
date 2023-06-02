using NanyPet.Api.Models;
using NanyPet.Models;
using System.Linq.Expressions;

namespace NanyPet.Repositories
{
    public interface IOwnerRepository
    {
        Task<List<Owner>> GetAllOwners(Expression<Func<Owner, bool>>? filter = null);
        Task<Owner?> GetOwnerById(Expression<Func<Owner, bool>>? filter = null, bool tracked = true);
        Task UpdateOwner(Owner owner);
        Task DeleteOwner(Owner owner);
        Task CreateOwner(Owner owner);

    }
}