
using System.Linq;
using System.Reflection;
using Autofac;
using Module = Autofac.Module;
using ZhonTai.Common.Domain;
using ZhonTai.Common.Domain.Repositories;
using Microsoft.Extensions.DependencyModel;

namespace ZhonTai.Admin.Core.RegisterModules
{
    public class RepositoryModule : Module
    {
        private readonly string _assemblySuffixName;

        /// <summary>
        /// 仓储注入
        /// </summary>
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
