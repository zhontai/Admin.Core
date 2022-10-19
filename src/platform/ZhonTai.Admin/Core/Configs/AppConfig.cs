using System.Collections.Generic;

namespace ZhonTai.Admin.Core.Configs;

/// <summary>
/// 应用配置
/// </summary>
public class AppConfig
{
    public AppType AppType { get; set; } = AppType.Controllers;

    /// <summary>
    /// Api地址，默认 http://*:8000
    /// </summary>
    public string[] Urls { get; set; }

    /// <summary>
    /// 跨域地址，默认 http://*:9000
    /// </summary>
    public string[] CorUrls { get; set; }

    /// <summary>
    /// 程序集名称
    /// </summary>
    public string[] AssemblyNames { get; set; }

    /// <summary>
    /// 租户类型
    /// </summary>
    public bool Tenant { get; set; } = false;

    /// <summary>
    /// 分布式事务唯一标识
    /// </summary>
    public string DistributeKey { get; set; }

    /// <summary>
    /// Swagger文档
    /// </summary>
    public SwaggerConfig Swagger { get; set; } = new SwaggerConfig();

    /// <summary>
    /// 新版Api文档
    /// </summary>
    public ApiUIConfig ApiUI { get; set; } = new ApiUIConfig();

    /// <summary>
    /// MiniProfiler性能分析器
    /// </summary>
    public bool MiniProfiler { get; set; } = false;

    /// <summary>
    /// 统一认证授权服务器
    /// </summary>
    public IdentityServer IdentityServer { get; set; } = new IdentityServer();

    /// <summary>
    /// Aop配置
    /// </summary>
    public AopConfig Aop { get; set; } = new AopConfig();

    /// <summary>
    /// 日志配置
    /// </summary>
    public LogConfig Log { get; set; } = new LogConfig();

    /// <summary>
    /// 验证配置
    /// </summary>
    public ValidateConfig Validate { get; set; } = new ValidateConfig();

    /// <summary>
    /// 限流
    /// </summary>
    public bool RateLimit { get; set; } = false;

    /// <summary>
    /// 验证码配置
    /// </summary>
    public VarifyCodeConfig VarifyCode { get; set; } = new VarifyCodeConfig();

    /// <summary>
    /// 默认密码
    /// </summary>
    public string DefaultPassword { get; set; } = "111111";
}

/// <summary>
/// Swagger配置
/// </summary>
public class SwaggerConfig
{
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = false;

    /// <summary>
    /// 地址
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 项目列表
    /// </summary>
    public List<ProjectConfig> Projects { get; set; }
}

/// <summary>
///新版Api文档配置
/// </summary>
public class ApiUIConfig
{
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = false;

    public SwaggerFooterConfig Footer { get; set; } = new SwaggerFooterConfig();
}

/// <summary>
/// Swagger页脚配置
/// </summary>
public class SwaggerFooterConfig
{
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = true;

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }
}

/// <summary>
/// 统一认证授权服务器配置
/// </summary>
public class IdentityServer
{
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = false;

    /// <summary>
    /// 地址
    /// </summary>
    public string Url { get; set; } = "https://localhost:5000";
}

/// <summary>
/// Aop配置
/// </summary>
public class AopConfig
{
    /// <summary>
    /// 事物
    /// </summary>
    public bool Transaction { get; set; } = true;
}

/// <summary>
/// 日志配置
/// </summary>
public class LogConfig
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public bool Operation { get; set; } = true;
}

/// <summary>
/// 验证配置
/// </summary>
public class ValidateConfig
{
    /// <summary>
    /// 登录
    /// </summary>
    public bool Login { get; set; } = true;

    /// <summary>
    /// 权限
    /// </summary>
    public bool Permission { get; set; } = true;
}


/// <summary>
/// 验证码配置
/// </summary>
public class VarifyCodeConfig
{
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = true;

    /// <summary>
    /// 操作日志
    /// </summary>
    public string[] Fonts { get; set; }// = new[] { "Times New Roman", "Verdana", "Arial", "Gungsuh", "Impact" };
}

/// <summary>
/// 项目配置
/// </summary>
public class ProjectConfig
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
}

/// <summary>
/// 应用程序类型
/// </summary>
public enum AppType
{
    Controllers,
    ControllersWithViews,
    MVC
}