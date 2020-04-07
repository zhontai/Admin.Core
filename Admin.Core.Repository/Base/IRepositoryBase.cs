
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FreeSql;

namespace Admin.Core.Repository
{
    public interface IRepositoryBase<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
    {
        /// <summary>
        /// 获得Dto
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TDto> GetAsync<TDto>(TKey id);

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp);

        /// <summary>
        /// 根据条件获取Dto
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        Task<TDto> GetAsync<TDto>(Expression<Func<TEntity, bool>> exp);

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> SoftDeleteAsync(TKey id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> SoftDeleteAsync(TKey[] id);
    }

    public interface IRepositoryBase<TEntity> : IRepositoryBase<TEntity,long> where TEntity : class
    {
    }

}
