using Autofac;
using System.Linq;
using System.Reflection;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Common.Helpers;
using Module = Autofac.Module;

namespace ZhonTai.Admin.Core.RegisterModules;

/// <summary>
/// 生命周期注入
/// </summary>
public class LifecycleModule : Module
{
    private readonly AppConfig _appConfig;

    public LifecycleModule(AppConfig appConfig)
    {
        _appConfig = appConfig;
    }

    protected override void Load(ContainerBuilder builder)
    {
        if(_appConfig.AssemblyNames?.Length > 0)
        {
            // 获得要注入的程序集
            Assembly[] assemblies = AssemblyHelper.GetAssemblyList(_appConfig.AssemblyNames);

            //无接口注入单例
            builder.RegisterAssemblyTypes(assemblies)
            .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>(false) != null)
            .SingleInstance()
            .PropertiesAutowired();

            //有接口注入单例
            builder.RegisterAssemblyTypes(assemblies)
            .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>(false) != null)
            .AsImplementedInterfaces()
            .SingleInstance()
            .PropertiesAutowired();

            //无接口注入单例
            builder.RegisterAssemblyTypes(assemblies)
            .Where(t => t.GetCustomAttribute<InjectSingletonAttribute>(false) != null)
            .SingleInstance()
            .PropertiesAutowired();

            //有接口注入单例
            builder.RegisterAssemblyTypes(assemblies)
            .Where(t => t.GetCustomAttribute<InjectSingletonAttribute>(false) != null)
            .AsImplementedInterfaces()
            .SingleInstance()
            .PropertiesAutowired();

            //无接口注入作用域
            builder.RegisterAssemblyTypes(assemblies)
            .Where(t => t.GetCustomAttribute<InjectScopedAttribute>(false) != null)
            .InstancePerLifetimeScope()
            .PropertiesAutowired();

            //有接口注入作用域
            builder.RegisterAssemblyTypes(assemblies)
            .Where(t => t.GetCustomAttribute<InjectScopedAttribute>(false) != null)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .PropertiesAutowired();

            //无接口注入瞬时
            builder.RegisterAssemblyTypes(assemblies)
            .Where(t => t.GetCustomAttribute<InjectTransientAttribute>(false) != null)
            .InstancePerDependency()
            .PropertiesAutowired();

            //有接口注入瞬时
            builder.RegisterAssemblyTypes(assemblies)
            .Where(t => t.GetCustomAttribute<InjectTransientAttribute>(false) != null)
            .AsImplementedInterfaces()
            .InstancePerDependency()
            .PropertiesAutowired();
        }
    }
}
