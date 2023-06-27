using Microsoft.EntityFrameworkCore;
using NanyPet.Api.Models;
using NanyPet.Api.Repositories;
using NanyPet.Api.Repositories.IRepository;
using System.Linq.Expressions;

namespace NanyPet.Repositories
{
    public class HerderRepository : Repository<Herder>, IHerderRepository
    {
        private readonly nanypetContext _context;

        public HerderRepository(nanypetContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Herder?> GetByEmail(Expression<Func<Herder, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<Herder> query = dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }
    }
}
