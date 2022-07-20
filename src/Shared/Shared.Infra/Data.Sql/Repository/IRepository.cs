using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Infra.Data.Sql.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<TEntity> Get(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Task<TEntity> Add(TEntity obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Task AddRange(List<TEntity> obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public TEntity Update(TEntity obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateRange(List<TEntity> obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void Delete(TEntity obj);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<List<TEntity>> GetAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objs"></param>
        public void DeleteRange(List<TEntity> objs);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task Commit();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task Rollback();
    }
}
