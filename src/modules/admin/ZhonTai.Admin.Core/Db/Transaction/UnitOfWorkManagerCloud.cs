using FreeSql;
using System.Collections.Concurrent;
using System.Data;

namespace ZhonTai.Admin.Core.Db.Transaction;

public class UnitOfWorkManagerCloud: IDisposable
{
    readonly ConcurrentDictionary<string, UnitOfWorkManager> _managers = new();
    readonly FreeSqlCloud _cloud;
    public UnitOfWorkManagerCloud(FreeSqlCloud cloud)
    {
        _cloud = cloud;
    }

    public UnitOfWorkManager GetUnitOfWorkManager(string dbKey)
    {
        try
        {
            return _managers.GetOrAdd(dbKey, new UnitOfWorkManager(_cloud.Use(dbKey)));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to get UnitOfWorkManager for database key: {dbKey}", ex);
        }
    }

    ~UnitOfWorkManagerCloud() => Dispose();

    int _disposeCounter;
    public void Dispose()
    {
        if (Interlocked.Increment(ref _disposeCounter) != 1) return;
        try
        {
            Exception ex = null;
            foreach (var uowm in _managers.Values)
            {
                try
                {
                    uowm.Dispose();
                }
                catch (Exception e)
                {
                    ex = e;
                }
            }

            if (ex != null)
            {
                throw ex;
            }
        }
        finally
        {
            _managers.Clear();
        }
    }

    public IUnitOfWork Begin(string dbKey, Propagation propagation = Propagation.Required, IsolationLevel? isolationLevel = null)
    {
        return GetUnitOfWorkManager(dbKey).Begin(propagation, isolationLevel);
    }
}