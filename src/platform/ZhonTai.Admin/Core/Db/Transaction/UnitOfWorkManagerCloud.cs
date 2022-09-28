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

    public UnitOfWorkManager GetUnitOfWorkManager(string db)
    {
        if (m_managers.TryGetValue(db, out var uowm) == false)
            m_managers.Add(db, uowm = new UnitOfWorkManager(m_cloud.Use(db)));
        return uowm;
    }

    public void Dispose()
    {
        foreach (var uowm in m_managers.Values) uowm.Dispose();
        m_managers.Clear();
    }

    public IUnitOfWork Begin(string db, Propagation propagation = Propagation.Required, IsolationLevel? isolationLevel = null)
    {
        return GetUnitOfWorkManager(db).Begin(propagation, isolationLevel);
    }
}