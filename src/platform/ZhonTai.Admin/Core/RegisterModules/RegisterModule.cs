using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;
using Microsoft.Extensions.DependencyModel;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Repositories;

namespace ZhonTai.Admin.Core.RegisterModules;

public class RegisterModule : Module
{
    private readonly AppConfig _appConfig;

    /// <summary>
    /// 模块注入
    /// </summary>
    /// <param name="appConfig">AppConfig</param>
    public RegisterModule(AppConfig appConfig)
    {
        _appConfig = appConfig;
    }

    protected override void Load(ContainerBuilder builder)
    {
        //事务拦截
        var interceptorServiceTypes = new List<Type>();
        if (_appConfig.Aop.Transaction)
        {
            builder.RegisterType<TransactionInterceptor>();
            builder.RegisterType<TransactionAsyncInterceptor>();
            interceptorServiceTypes.Add(typeof(TransactionInterceptor));
        }

        if(_appConfig.AssemblyNames?.Length > 0)
        {
            //程序集
            Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
                .Where(a => _appConfig.AssemblyNames.Contains(a.Name))
                .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();

            var nonRegisterIOCAttribute = typeof(NonRegisterIOCAttribute);
            var iRegisterIOCType = typeof(IRegisterIOC);

            bool Predicate(Type a) => !a.IsDefined(nonRegisterIOCAttribute, true) 
                && (a.Name.EndsWith("Service") || a.Name.EndsWith("Repository") || iRegisterIOCType.IsAssignableFrom(a)) 
                && !a.IsAbstract && !a.IsInterface && a.IsPublic;

            //有接口实例
            builder.RegisterAssemblyTypes(assemblies)
            .Where(Predicate)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .PropertiesAutowired()// 属性注入
            .InterceptedBy(interceptorServiceTypes.ToArray())
            .EnableInterfaceInterceptors();

            //无接口实例
            builder.RegisterAssemblyTypes(assemblies)
            .Where(Predicate)
            .InstancePerLifetimeScope()
            .PropertiesAutowired()// 属性注入
            .InterceptedBy(interceptorServiceTypes.ToArray())
            .EnableClassInterceptors();

            //仓储泛型注入
            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>)).InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepositoryBase<,>)).InstancePerLifetimeScope().PropertiesAutowired();
        }
    }
}
