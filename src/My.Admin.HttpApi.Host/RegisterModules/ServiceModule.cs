
using My.Admin.HttpApi.Host.Aop;
using My.Admin.Common.Configs;
using My.Admin.Repository;
using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;

namespace My.Admin.HttpApi.Host.RegisterModules
{
    public class ServiceModule : Module
    {
        private readonly AppConfig _appConfig;
        private readonly string _assemblyName;
        private readonly string _suffixName;

        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="appConfig">AppConfig</param>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="suffixName">后缀名</param>
        public ServiceModule(AppConfig appConfig, string assemblyName = "My.Admin.Service", string suffixName = "Service")
        {
            _appConfig = appConfig;
            _assemblyName = assemblyName;
            _suffixName = suffixName;
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

            //服务
            var assemblyServices = Assembly.Load(_assemblyName);
            builder.RegisterAssemblyTypes(assemblyServices)
            .Where(a => a.Name.EndsWith(_suffixName))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .PropertiesAutowired()// 属性注入
            .InterceptedBy(interceptorServiceTypes.ToArray())
            .EnableInterfaceInterceptors();
        }
    }
}
