using FreeSql;
using System;
using System.Linq.Expressions;
using ZhonTai.Admin.Core.Db;

namespace ZhonTai.Admin.Core.Repositories;

public class DefaultRepositoryBase<TEntity, TKey> : RepositoryBase<TEntity, TKey> where TEntity : class
{
    public DefaultRepositoryBase(IFreeSql fsql) : base(fsql, null, null) { }
    public DefaultRepositoryBase(IFreeSql fsql, Expression<Func<TEntity, bool>> filter) : base(fsql, filter, null) { }
    public DefaultRepositoryBase(IFreeSql fsql, UnitOfWorkManager uowManger) : base(uowManger?.Orm ?? fsql, null, null)
    {
        uowManger?.Binding(this);
    }
}

public class RepositoryCloud<T> : DefaultRepositoryBase<T, long> where T : class
{
    public RepositoryCloud(UnitOfWorkManagerCloud uomw) : this("master", uomw) { }
    public RepositoryCloud(string db, UnitOfWorkManagerCloud uomw) : this(uomw.GetUnitOfWorkManager(db)) { }
    RepositoryCloud(UnitOfWorkManager uomw) : base(uomw.Orm, uomw)
    {
        uomw.Binding(this);
    }
}