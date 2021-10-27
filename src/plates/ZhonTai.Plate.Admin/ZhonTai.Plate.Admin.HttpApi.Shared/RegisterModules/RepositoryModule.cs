
using System.Linq;
using System.Reflection;
using Autofac;
using Module = Autofac.Module;
using ZhonTai.Plate.Admin.Domain;
using ZhonTai.Common.Domain.Repositories;
using Microsoft.Extensions.DependencyModel;

namespace ZhonTai.Plate.Admin.HttpApi.Shared.RegisterModules
{
    public class RepositoryModule : Module
    {
        private readonly string _assemblySuffixName;

        /// <summary>
        /// 仓储注入
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="assemblySuffixName">程序集后缀名</param>
        public RepositoryModule(string assemblySuffixName = "Repository")
        {
            _assemblySuffixName = assemblySuffixName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //仓储
            Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
                .Where(a => a.Name.EndsWith(_assemblySuffixName))
                .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();

            builder.RegisterAssemblyTypes(assemblies)
            .Where(a => a.Name.EndsWith(_assemblySuffixName))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .PropertiesAutowired();// 属性注入

            //泛型注入
            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepositoryBase<,>)).InstancePerLifetimeScope();
        }
    }
}
