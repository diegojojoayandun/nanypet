using Microsoft.EntityFrameworkCore;
using NanyPet.Api.Models;
using NanyPet.Api.Repositories;
using System.Linq.Expressions;

namespace NanyPet.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly nanypetContext _context;

        public UserRepository(nanypetContext context) : base(context)
        {
            _context = context;
            dbSet = _context.Set<User>();

        }

        public async Task<User?> GetUserByEmail(Expression<Func<User, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<User> query = dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }
    }
}
