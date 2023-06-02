using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using NanyPet.Api.Models;
using NanyPet.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace NanyPet.Repositories
{
    public class UserRepository : IUserRepository
    {
        // private readonly MySQLConfiguration _connectionString;
        private readonly nanypetContext _context;
        internal DbSet<User> dbSet;

        public UserRepository(nanypetContext context)
        {
            //_connectionString = connectionString;
            _context = context;
            dbSet = _context.Set<User>();

        }

        /*protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);

        }*/

        //public async Task DeleteHerder(Herder herder)
        //{
        //    _context.Remove(herder);
        //    await _context.SaveChangesAsync();
        //}

        public async Task<List<User>> GetAllUsers(Expression<Func<User, bool>>? filter = null)
        {
            /*IEnumerable<Herder> herderList = _context.Herders.ToList();
            return herderList;*/

            IQueryable<User> query = dbSet;


            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();

        }

        public async Task<User?> GetUserById(Expression<Func<User, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<User> query = dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();


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

        //public Task InsertHerder(Herder herder)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task UpdateHerder(Herder herder)
        //{
        //    _context.Update(herder);
        //    await _context.SaveChangesAsync();
        //}

        public async Task CreateUser(User user)
        {
            await dbSet.AddAsync(user);
            await _context.SaveChangesAsync();
        }


    }
}
