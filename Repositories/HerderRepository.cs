using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using NanyPet.Api.Models;
using NanyPet.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace NanyPet.Repositories
{
    public class HerderRepository : IHerderRepository
    {
        // private readonly MySQLConfiguration _connectionString;
        private readonly nanypetContext _context;
        internal DbSet<Herder> dbSet;

        public HerderRepository(nanypetContext context)
        {
            //_connectionString = connectionString;
            _context = context;
            dbSet = _context.Set<Herder>();

        }

        /*protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);

        }*/

        public async Task DeleteHerder(Herder herder)
        {
            _context.Remove(herder);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Herder>> GetAllHerders(Expression<Func<Herder, bool>>? filter = null)
        {
            /*IEnumerable<Herder> herderList = _context.Herders.ToList();
            return herderList;*/

            IQueryable<Herder> query = dbSet;


            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();

        }

        public async Task<Herder?> GetHerderById(Expression<Func<Herder, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<Herder> query = dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();


        }

        public Task InsertHerder(Herder herder)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateHerder(Herder herder)
        {
            _context.Update(herder);
            await _context.SaveChangesAsync();
        }

        public async Task CreateHerder(Herder herder)
        {
            await dbSet.AddAsync(herder);
            await _context.SaveChangesAsync();
        }


    }
}
