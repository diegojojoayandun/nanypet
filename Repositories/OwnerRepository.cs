using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using NanyPet.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace NanyPet.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly nanypetContext _context;
        internal DbSet<Owner> dbSet;

        public OwnerRepository(nanypetContext context)
        {
            _context = context;
            dbSet = _context.Set<Owner>();

        }

        public async Task DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Owner>> GetAllOwners(Expression<Func<Owner, bool>>? filter = null)
        {

            IQueryable<Owner> query = dbSet;


            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();

        }

        public async Task<Owner?> GetOwnerById(Expression<Func<Owner, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<Owner> query = dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();


        }


        public async Task UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            await _context.SaveChangesAsync();
        }

        public async Task CreateOwner(Owner owner)
        {
            await dbSet.AddAsync(owner);
            await _context.SaveChangesAsync();
        }


    }
}
