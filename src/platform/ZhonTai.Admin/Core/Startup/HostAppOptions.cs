using FreeSql;
using System;
using Yitter.IdGenerator;
using ZhonTai.DynamicApi;

namespace ZhonTai.Admin.Core.Startup;

/// <summary>
/// 宿主应用配置
/// </summary>
public class HostAppOptions
{
    /// <summary>
    /// 注入前置服务
    /// </summary>
    public Action<HostAppContext> ConfigurePreServices { get; set; }

    /// <summary>
    /// 注入服务
    /// </summary>
    public Action<HostAppContext> ConfigureServices { get; set; }

    /// <summary>
    /// 注入后置服务
    /// </summary>
    public Action<HostAppContext> ConfigurePostServices { get; set; }

    /// <summary>
    /// 注入前置中间件
    /// </summary>
    public Action<HostAppMiddlewareContext> ConfigurePreMiddleware { get; set; }

    /// <summary>
    /// 注入中间件
    /// </summary>
    public Action<HostAppMiddlewareContext> ConfigureMiddleware { get; set; }

    /// <summary>
    /// 注入后置中间件
    /// </summary>
    public Action<HostAppMiddlewareContext> ConfigurePostMiddleware { get; set; }

    /// <summary>
    /// 配置主库FreeSql构建器
    /// </summary>
    public Action<FreeSqlBuilder> ConfigureFreeSqlBuilder { get; set; }

    /// <summary>
    /// 配置主库FreeSql
    /// </summary>
    public Action<IFreeSql> ConfigureFreeSql { get; set; }

    /// <summary>
    /// 配置动态Api
    /// </summary>
    public Action<DynamicApiOptions> ConfigureDynamicApi { get; set; }

    /// <summary>
    /// 配置雪花漂移算法
    /// </summary>
    public Action<IdGeneratorOptions> ConfigureIdGenerator { get; set; }

    /// <summary>
    /// 自定义数据库初始化
    /// </summary>
    public bool CustomInitDb { get; set; } = false;

}