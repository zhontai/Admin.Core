using FreeSql;
using System.Collections.Generic;
using System.Data;

namespace ZhonTai.Admin.Core.Db.Transaction;

public class UnitOfWorkManagerCloud
{
    readonly Dictionary<string, UnitOfWorkManager> _managers = new Dictionary<string, UnitOfWorkManager>();
    readonly FreeSqlCloud _cloud;
    public UnitOfWorkManagerCloud(FreeSqlCloud cloud)
    {
        _cloud = cloud;
    }

    public UnitOfWorkManager GetUnitOfWorkManager(string dbKey)
    {
        if (_managers.TryGetValue(dbKey, out var uowm) == false)
        {
            _managers.Add(dbKey, uowm = new UnitOfWorkManager(_cloud.Use(dbKey)));
        }
            
        return uowm;
    }

    public void Dispose()
    {
        foreach (var uowm in _managers.Values) uowm.Dispose();
        _managers.Clear();
    }

    public IUnitOfWork Begin(string dbKey, Propagation propagation = Propagation.Required, IsolationLevel? isolationLevel = null)
    {
        return GetUnitOfWorkManager(dbKey).Begin(propagation, isolationLevel);
    }
}