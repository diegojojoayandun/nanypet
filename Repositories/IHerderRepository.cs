using NanyPet.Api.Models;
using NanyPet.Models;
using System.Linq.Expressions;

namespace NanyPet.Repositories
{
    public interface IHerderRepository
    {
        Task<List<Herder>> GetAllHerders(Expression<Func<Herder, bool>>? filter = null);
        Task<Herder?> GetHerderById(Expression<Func<Herder, bool>>? filter = null, bool tracked = true);
        Task InsertHerder(Herder herder);
        Task UpdateHerder(Herder herder);
        Task DeleteHerder(Herder herder);
        Task CreateHerder(Herder herder);

    }
}