using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ZhonTai.DynamicApi;

public interface IActionRouteFactory
{
    string CreateActionRouteModel(string areaName, string controllerName, ActionModel action);
}

internal class DefaultActionRouteFactory : IActionRouteFactory
{
    private static string GetApiPreFix(ActionModel action)
    {
        var getValueSuccess = AppConsts.AssemblyDynamicApiOptions
            .TryGetValue(action.Controller.ControllerType.Assembly, out AssemblyDynamicApiOptions assemblyDynamicApiOptions);
        if (getValueSuccess && !string.IsNullOrWhiteSpace(assemblyDynamicApiOptions?.ApiPrefix))
        {
            return assemblyDynamicApiOptions.ApiPrefix;
        }

        return AppConsts.DefaultApiPreFix;
    }

    public string CreateActionRouteModel(string areaName, string controllerName, ActionModel action)
    {
        var apiPreFix = GetApiPreFix(action);
        var routeStr = $"{apiPreFix}/{areaName}/{controllerName}/{action.ActionName}".Replace("//", "/");
        return routeStr;        }
}