
using Admin.Core.Common.Attributes;
using Autofac;
using Microsoft.Extensions.DependencyModel;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;

namespace Admin.Core.RegisterModules
{
    public class SingleInstanceModule : Module
    {
        private readonly string _prefixName;

        /// <summary>
        /// 单例注入
        /// </summary>
        /// <param name="prefixName">前缀名</param>
        public SingleInstanceModule(string prefixName = "Admin.")
        {
            _prefixName = prefixName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // 获得要注入的程序集
            Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
                .Where(a => a.Name.StartsWith(_prefixName))
                .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();

            //无接口注入单例
            builder.RegisterAssemblyTypes(assemblies)
            .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>() != null)
            .SingleInstance()
            .PropertiesAutowired();

            //有接口注入单例
            builder.RegisterAssemblyTypes(assemblies)
            .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>() != null)
            .AsImplementedInterfaces()
            .SingleInstance()
            .PropertiesAutowired();
        }
    }
}
