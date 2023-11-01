using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FreeSql;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Auth;
using ZhonTai.Admin.Core.Startup;
using System.Collections.Concurrent;
using System.Reflection;
using ZhonTai.Admin.Core.Db.Transaction;

namespace ZhonTai.Admin.Core.Db;

public static class DBServiceCollectionExtensions
{
    /// <summary>
    /// 添加数据库
    /// </summary>
    /// <param name="services"></param>
    /// <param name="env"></param>
    /// <param name="hostAppOptions"></param>
    /// <returns></returns>
    public static void AddDb(this IServiceCollection services, IHostEnvironment env, HostAppOptions hostAppOptions)
    {
        var dbConfig = ConfigHelper.Get<DbConfig>("dbconfig", env.EnvironmentName);
        var appConfig = ConfigHelper.Get<AppConfig>("appconfig", env.EnvironmentName);
        var user = services.BuildServiceProvider().GetService<IUser>();
        var freeSqlCloud = appConfig.DistributeKey.IsNull() ? new FreeSqlCloud() : new FreeSqlCloud(appConfig.DistributeKey);
        DbHelper.RegisterDb(freeSqlCloud, user, dbConfig, appConfig, hostAppOptions);

        //运行主库
        var masterDb = freeSqlCloud.Use(dbConfig.Key);
        services.AddSingleton(provider => masterDb);
        masterDb.Select<object>();

        //注册多数据库
        if (dbConfig.Dbs?.Length > 0)
        {
            foreach (var db in dbConfig.Dbs)
            {
                DbHelper.RegisterDb(freeSqlCloud, user, db, appConfig, null);
                //运行当前库
                var currentDb = freeSqlCloud.Use(dbConfig.Key);
                currentDb.Select<object>();
            }
        }

        services.AddSingleton<IFreeSql>(freeSqlCloud);
        services.AddSingleton(freeSqlCloud);
        services.AddScoped<UnitOfWorkManagerCloud>();
    }

    /// <summary>
    /// 添加TiDb数据库
    /// </summary>
    /// <param name="_"></param>
    /// <param name="context"></param>
    /// <param name="version">版本</param>
    public static void AddTiDb(this IServiceCollection _, HostAppContext context, string version = "8.0")
    {
        var dbConfig = ConfigHelper.Get<DbConfig>("dbconfig", context.Environment.EnvironmentName);
        var _dicMySqlVersion = typeof(FreeSqlGlobalExtensions).GetField("_dicMySqlVersion", BindingFlags.NonPublic | BindingFlags.Static);
        var dicMySqlVersion = new ConcurrentDictionary<string, string>();
        dicMySqlVersion[dbConfig.ConnectionString] = version;
        _dicMySqlVersion.SetValue(new ConcurrentDictionary<string, string>(), dicMySqlVersion);
    }
}