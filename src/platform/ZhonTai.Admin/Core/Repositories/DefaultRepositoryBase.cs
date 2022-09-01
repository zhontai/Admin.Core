using System;
using System.Linq.Expressions;
using ZhonTai.Admin.Core.Db;

namespace ZhonTai.Admin.Core.Repositories;

public class DefaultRepositoryBase<TEntity, TKey> : RepositoryBase<TEntity, TKey> where TEntity : class, new()
{
    public DefaultRepositoryBase(IFreeSql fsql) : base(fsql) { }
    public DefaultRepositoryBase(IFreeSql fsql, Expression<Func<TEntity, bool>> filter) : base(fsql, filter, null) { }
    public DefaultRepositoryBase(IFreeSql fsql, DbUnitOfWorkManager muowManger) : base(muowManger?.Orm ?? fsql, null, null)
    {
        muowManger?.Binding(this);
    }
}
