
using System.Linq;
using System.Reflection;
using Autofac;
using Module = Autofac.Module;
using ZhonTai.Plate.Admin.Repository;
using ZhonTai.Common.Domain.Repositories;

namespace ZhonTai.HttpApi.RegisterModules
{
    public class RepositoryModule : Module
    {
        private readonly string _assemblyName;
        private readonly string _assemblySuffixName;

        /// <summary>
        /// 仓储注入
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="assemblySuffixName">程序集后缀名</param>
        public RepositoryModule(string assemblyName = "ZhonTai.Plate.Admin.Repository", string assemblySuffixName = "Repository")
        {
            _assemblyName = assemblyName;
            _assemblySuffixName = assemblySuffixName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //仓储
            var assemblyRepository = Assembly.Load(_assemblyName);
            builder.RegisterAssemblyTypes(assemblyRepository)
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
