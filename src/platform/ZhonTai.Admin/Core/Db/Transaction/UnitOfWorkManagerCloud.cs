using FreeSql;
using System;
using System.Collections.Generic;
using System.Data;
using ZhonTai.Admin.Core.Auth;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;

namespace ZhonTai.Admin.Core.Db.Transaction;

public class UnitOfWorkManagerCloud
{
    readonly Dictionary<string, UnitOfWorkManager> _managers = new Dictionary<string, UnitOfWorkManager>();
    readonly FreeSqlCloud _cloud;
    readonly IUser _user;
    readonly AppConfig _appConfig;
    readonly IServiceProvider _serviceProvider;
    public UnitOfWorkManagerCloud(
        FreeSqlCloud cloud, 
        IUser user, 
        AppConfig appConfig, 
        IServiceProvider serviceProvider)
    {
        _cloud = cloud;
        _user = user;
        _appConfig = appConfig;
        _serviceProvider = serviceProvider;
    }

    public UnitOfWorkManager GetUnitOfWorkManager(string dbKey)
    {
        if (dbKey.IsNull())
        {
            if (_appConfig.Tenant && _user.TenantId.HasValue)
            {
                dbKey = _user.DbKey.NotNull() ? _user.DbKey : (DbKeys.TenantDbKey + _user.TenantId);
                _cloud.GetCurrentDb(_serviceProvider);
            }
            else
            {
                dbKey = DbKeys.MasterDbKey;
            }
        }
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