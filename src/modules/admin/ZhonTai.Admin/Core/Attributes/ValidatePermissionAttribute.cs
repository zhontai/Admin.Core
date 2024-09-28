using BaiduBce.Services.Bos.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;  
using ZhonTai.Admin.Core.Auth;
using ZhonTai.Admin.Core.Configs;

namespace ZhonTai.Admin.Core.Attributes;

/// <summary>
/// 启用权限验证
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ValidatePermissionAttribute : AuthorizeAttribute, IAuthorizationFilter, IAsyncAuthorizationFilter
{
    private async Task PermissionAuthorization(AuthorizationFilterContext context)
    {
        //排除匿名访问
        if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(AllowAnonymousAttribute)))
            return;

        var serviceProvider = context.HttpContext.RequestServices;
        //登录验证
        var user = serviceProvider.GetService<IUser>();
        if (user == null || !(user?.Id > 0))
        {
            context.Result = new ChallengeResult();
            return;
        }

        //排除登录接口
        if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(LoginAttribute)))
            return;

        if (user.PlatformAdmin)
        {
            return;
        }

        //权限验证
        if (serviceProvider.GetRequiredService<AppConfig>().Validate.Permission)
        {
            var apiAccess = context.HttpContext.GetEndpoint()?.Metadata?.GetMetadata<ApiAccessAttribute>();
            
            var httpMethod = context.HttpContext.Request.Method;
            var api = context.ActionDescriptor.AttributeRouteInfo.Template;
            var permissionHandler = serviceProvider.GetService<IPermissionHandler>();
            var isValid = await permissionHandler.ValidateAsync(api, httpMethod, apiAccess);
            if (!isValid)
            {
                context.Result = new ForbidResult();
            }
        }
    }

    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        await PermissionAuthorization(context);
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        await PermissionAuthorization(context);
    }
}