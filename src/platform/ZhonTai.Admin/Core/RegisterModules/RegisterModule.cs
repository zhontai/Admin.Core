using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using ZhonTai.Common.Helpers;

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
            Assembly[] assemblies = AssemblyHelper.GetAssemblyList(_appConfig.AssemblyNames);

            static bool Predicate(Type a) => !a.IsDefined(typeof(NonRegisterIOCAttribute), true) 
                && (a.Name.EndsWith("Service") || a.Name.EndsWith("Repository") || typeof(IRegisterIOC).IsAssignableFrom(a)) 
                && !a.IsAbstract && !a.IsInterface && a.IsPublic;

            //有接口实例
            builder.RegisterAssemblyTypes(assemblies)
            .Where(new Func<Type, bool>(Predicate))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .PropertiesAutowired()// 属性注入
            .InterceptedBy(interceptorServiceTypes.ToArray())
            .EnableInterfaceInterceptors();

            //无接口实例
            builder.RegisterAssemblyTypes(assemblies)
            .Where(new Func<Type, bool>(Predicate))
            .InstancePerLifetimeScope()
            .PropertiesAutowired()// 属性注入
            .InterceptedBy(interceptorServiceTypes.ToArray())
            .EnableClassInterceptors();

            //密码哈希泛型注入
            builder.RegisterGeneric(typeof(PasswordHasher<>)).As(typeof(IPasswordHasher<>)).SingleInstance().PropertiesAutowired();

            //仓储泛型注入
            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>)).InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepositoryBase<,>)).InstancePerLifetimeScope().PropertiesAutowired();
        }
    }
}
