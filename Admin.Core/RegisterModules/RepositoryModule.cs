
using Admin.Core.Repository;
using Autofac;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;

namespace Admin.Core.RegisterModules
{
    public class RepositoryModule : Module
    {
        private readonly string _assemblyName;
        private readonly string _suffixName;

        /// <summary>
        /// 仓储注入
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="suffixName">后缀名</param>
        public RepositoryModule(string assemblyName = "Admin.Core.Repository", string suffixName = "Repository")
        {
            _assemblyName = assemblyName;
            _suffixName = suffixName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //仓储
            var assemblyRepository = Assembly.Load(_assemblyName);
            builder.RegisterAssemblyTypes(assemblyRepository)
            .Where(a => a.Name.EndsWith(_suffixName))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .PropertiesAutowired();// 属性注入

            //泛型注入
            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepositoryBase<,>)).InstancePerLifetimeScope();
        }
    }
}
