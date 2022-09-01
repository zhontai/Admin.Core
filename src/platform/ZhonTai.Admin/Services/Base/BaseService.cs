using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MapsterMapper;
using ZhonTai.Admin.Core.Auth;
using ZhonTai.Admin.Tools.Cache;
using ZhonTai.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ZhonTai.Admin.Services;

public abstract class BaseService: IBaseService
{
    protected readonly object ServiceProviderLock = new object();
    protected IDictionary<Type, object> CachedServices = new Dictionary<Type, object>();
    private ICacheTool _cache;
    private ILoggerFactory _loggerFactory;
    private IMapper _mapper;
    private IUser _user;

    /// <summary>
    /// 缓存
    /// </summary>
    public ICacheTool Cache => LazyGetRequiredService(ref _cache);

    /// <summary>
    /// 日志工厂
    /// </summary>
    public ILoggerFactory LoggerFactory => LazyGetRequiredService(ref _loggerFactory);

    /// <summary>
    /// 映射
    /// </summary>
    public IMapper Mapper => LazyGetRequiredService(ref _mapper);

    public IServiceProvider ServiceProvider { get; set; }

    /// <summary>
    /// 用户信息
    /// </summary>
    public IUser User => LazyGetRequiredService(ref _user);

    /// <summary>
    /// 日志
    /// </summary>
    protected ILogger Logger => _lazyLogger.Value;

    private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);

    protected TService LazyGetRequiredService<TService>(ref TService reference)
    {
        if (reference == null)
        {
            lock (ServiceProviderLock)
            {
                if (reference == null)
                {
                    reference = ServiceProvider.GetRequiredService<TService>();
                }
            }
        }

        return reference;
    }

    /// <summary>
    /// 获得懒加载服务
    /// </summary>
    /// <typeparam name="TService">服务接口</typeparam>
    /// <returns></returns>
    [NonAction]
    public virtual TService LazyGetRequiredService<TService>()
    {
        return (TService)LazyGetRequiredService(typeof(TService));
    }

    /// <summary>
    /// 根据服务类型获得懒加载服务
    /// </summary>
    /// <param name="serviceType">服务类型</param>
    /// <returns></returns>
    [NonAction]
    public virtual object LazyGetRequiredService(Type serviceType)
    {
        return CachedServices.GetOrAdd(serviceType, () => ServiceProvider.GetRequiredService(serviceType));
    }
}