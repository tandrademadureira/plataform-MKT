using Microsoft.EntityFrameworkCore;
using Shared.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Infra.Data.Sql.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class RepositoryBase<TContext, TEntity> : IRepository<TEntity>
        where TContext : DbContext
        where TEntity : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly TContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected RepositoryBase(TContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<TEntity> Add(TEntity obj)
        {
            var entityEntry = await _context.AddAsync(obj);
            return entityEntry.Entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public async Task AddRange(List<TEntity> objs)
        {
            await _context.Set<TEntity>().AddRangeAsync(objs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> Get(Guid id)
        {
            var data = await _context.Set<TEntity>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<TEntity>> GetAll()
        {
            var data = await _context.Set<TEntity>().ToListAsync();
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public TEntity Update(TEntity obj)
        {
            var data = _context.Set<TEntity>().Update(obj);
            data.State = EntityState.Modified;
            return data.Entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objs"></param>
        public void UpdateRange(List<TEntity> objs)
        {
            _context.Set<TEntity>().UpdateRange(objs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void Delete(TEntity obj)
        {
            _context.Set<TEntity>().Remove(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objs"></param>
        public void DeleteRange(List<TEntity> objs)
        {
            _context.Set<TEntity>().RemoveRange(objs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Commit()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    var proposedValues = entry.CurrentValues;
                    var databaseValues = await entry.GetDatabaseValuesAsync();

                    foreach (var property in proposedValues.Properties)
                    {
                        var proposedValue = proposedValues[property];
                        var databaseValue = databaseValues[property];
                        //saving most recent values
                        proposedValues[property] = proposedValue;
                    }

                    // Refresh original values to bypass next concurrency check
                    entry.OriginalValues.SetValues(databaseValues);

                }

            }

            //trying to save again
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task Rollback()
        {
            return Task.CompletedTask;
        }
    }
}
