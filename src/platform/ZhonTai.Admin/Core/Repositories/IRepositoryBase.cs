using FreeSql;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Auth;

namespace ZhonTai.Admin.Core.Repositories;

public interface IRepositoryBase<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
{
    IUser User { get; set; }

    /// <summary>
    /// 获得Dto
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<TDto> GetAsync<TDto>(TKey id);

    /// <summary>
    /// 根据条件获取Dto
    /// </summary>
    /// <param name="exp"></param>
    /// <returns></returns>
    Task<TDto> GetAsync<TDto>(Expression<Func<TEntity, bool>> exp);

    /// <summary>
    /// 根据条件获取实体
    /// </summary>
    /// <param name="exp"></param>
    /// <returns></returns>
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp);

    /// <summary>
    /// 软删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<bool> SoftDeleteAsync(TKey id);

    /// <summary>
    /// 批量软删除
    /// </summary>
    /// <param name="ids">主键数组</param>
    /// <returns></returns>
    Task<bool> SoftDeleteAsync(TKey[] ids);

    /// <summary>
    /// 软删除
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="disableGlobalFilterNames">禁用全局过滤器名</param>
    /// <returns></returns>
    Task<bool> SoftDeleteAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames);

    /// <summary>
    /// 递归删除
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="disableGlobalFilterNames">禁用全局过滤器名</param>
    /// <returns></returns>
    Task<bool> DeleteRecursiveAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames);

    /// <summary>
    /// 递归软删除
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="disableGlobalFilterNames">禁用全局过滤器名</param>
    /// <returns></returns>
    Task<bool> SoftDeleteRecursiveAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames);
}

public interface IRepositoryBase<TEntity> : IRepositoryBase<TEntity, long> where TEntity : class
{
}