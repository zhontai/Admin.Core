
using System.Linq;
using System.Reflection;
using Autofac;
using Module = Autofac.Module;
using Microsoft.Extensions.DependencyModel;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Core.Configs;

namespace ZhonTai.Admin.Core.RegisterModules
{
    public class RepositoryModule : Module
    {
        private readonly AppConfig _appConfig;

        /// <summary>
        /// 仓储注入
        /// </summary>
        /// <param name="appConfig">AppConfig</param>
        public RepositoryModule(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        protected override void Load(ContainerBuilder builder)
        {
            

            if(_appConfig.AssemblyNames?.Length > 0)
            {
                //仓储
                Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
                    .Where(a => _appConfig.AssemblyNames.Contains(a.Name))
                    .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();

                builder.RegisterAssemblyTypes(assemblies)
                .Where(a => a.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();// 属性注入
            }
            

            //泛型注入
            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>)).InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepositoryBase<,>)).InstancePerLifetimeScope().PropertiesAutowired();
        }
    }
}
