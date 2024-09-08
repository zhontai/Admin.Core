
using Autofac;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;

namespace ZhonTai.Admin.Core.RegisterModules;

public class ControllerModule : Module
{
    /// <summary>
    /// 控制器注入
    /// </summary>
    public ControllerModule()
    {
    }

    protected override void Load(ContainerBuilder builder)
    {
        var controllerTypes = Assembly.GetExecutingAssembly().GetExportedTypes()
        .Where(a => typeof(ControllerBase).IsAssignableFrom(a) && !a.IsAbstract && !a.IsInterface && a.IsPublic)
        .ToArray();

        // 配置所有控制器均支持属性注入
        builder.RegisterTypes(controllerTypes).PropertiesAutowired();
    }
}
