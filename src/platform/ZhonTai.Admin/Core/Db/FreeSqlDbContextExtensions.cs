using System;
using ZhonTai.Admin.Core.Repositories;
using System.Linq.Expressions;

namespace ZhonTai.Admin.Core.Db;

public static class FreeSqlDbContextExtensions
{
    /// <summary>
    /// 返回默认仓库类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="that"></param>
    /// <param name="filter">数据过滤 + 验证</param>
    /// <returns></returns>
    public static IRepositoryBase<TEntity, TKey> GetRepositoryBase<TEntity, TKey>(this IFreeSql that, Expression<Func<TEntity, bool>> filter = null) where TEntity : class
    {
        return new RepositoryBase<TEntity, TKey>(that, filter);
    }

    /// <summary>
    /// 返回默认仓库类，适用联合主键的仓储类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="that"></param>
    /// <param name="filter">数据过滤 + 验证</param>
    /// <returns></returns>
    public static IRepositoryBase<TEntity, long> GetRepositoryBase<TEntity>(this IFreeSql that, Expression<Func<TEntity, bool>> filter = null) where TEntity : class
    {
        return new RepositoryBase<TEntity, long>(that, filter);
    }
}