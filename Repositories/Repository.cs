﻿using Microsoft.EntityFrameworkCore;
using NanyPet.Api.Models;
using NanyPet.Api.Models.Specifications;
using NanyPet.Api.Repositories.IRepository;
using System.Linq.Expressions;

namespace NanyPet.Api.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly nanypetContext _context;
        internal DbSet<T> dbSet;

        public Repository(nanypetContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }
        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();
        }

        public PagedList<T> GetAllPaginated(Parameters parameters, Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            return PagedList<T>.ToPagedList(query, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<T?> GetById(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        public async Task Update(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
