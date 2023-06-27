using Microsoft.EntityFrameworkCore;
using NanyPet.Api.Models;
using NanyPet.Api.Repositories;
using System.Linq.Expressions;

namespace NanyPet.Repositories
{
    public class OwnerRepository : Repository<Owner>, IOwnerRepository
    {
        private readonly nanypetContext _context;

        public OwnerRepository(nanypetContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Owner?> GetByEmail(Expression<Func<Owner, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<Owner> query = dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }
    }
}
