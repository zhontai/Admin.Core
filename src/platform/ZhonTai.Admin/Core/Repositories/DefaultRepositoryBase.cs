using FreeSql;
using System;
using System.Linq.Expressions;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;

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
    public RepositoryCloud(UnitOfWorkManagerCloud uowm) : this(DbKeys.MasterDbKey, uowm) { }
    public RepositoryCloud(string db, UnitOfWorkManagerCloud uowm) : this(uowm.GetUnitOfWorkManager(db)) { }
    RepositoryCloud(UnitOfWorkManager uowm) : base(uowm.Orm, uowm)
    {
        uowm.Binding(this);
    }
}