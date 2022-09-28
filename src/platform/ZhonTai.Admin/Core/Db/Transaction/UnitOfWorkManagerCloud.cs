using FreeSql;
using System.Collections.Generic;
using System.Data;

namespace ZhonTai.Admin.Core.Db.Transaction;

public class UnitOfWorkManagerCloud
{
    readonly Dictionary<string, UnitOfWorkManager> m_managers = new Dictionary<string, UnitOfWorkManager>();
    readonly FreeSqlCloud m_cloud;
    public UnitOfWorkManagerCloud(FreeSqlCloud cloud)
    {
        m_cloud = cloud;
    }

    public UnitOfWorkManager GetUnitOfWorkManager(string dbKey)
    {
        if (m_managers.TryGetValue(dbKey, out var uowm) == false)
            m_managers.Add(dbKey, uowm = new UnitOfWorkManager(m_cloud.Use(dbKey)));
        return uowm;
    }

    public void Dispose()
    {
        foreach (var uowm in m_managers.Values) uowm.Dispose();
        m_managers.Clear();
    }

    public IUnitOfWork Begin(string dbKey, Propagation propagation = Propagation.Required, IsolationLevel? isolationLevel = null)
    {
        return GetUnitOfWorkManager(dbKey).Begin(propagation, isolationLevel);
    }
}