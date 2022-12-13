using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Exceptions;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Common.Helpers;

namespace ZhonTai.Admin.Core.Middlewares;

/// <summary>
/// 异常中间件
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (AppException ex)
        {
            await HandleAppExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleAppExceptionAsync(HttpContext context, AppException appException)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.OK;

        //_logger.LogError(exception, "");

        return context.Response.WriteAsync(JsonHelper.Serialize(new ResultOutput<string>()
        {
            Code = appException.AppCode
        }.NotOk(appException.AppMessage)));
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        _logger.LogError(exception, "");

        return context.Response.WriteAsync(JsonHelper.Serialize(new ResultOutput<string>().NotOk(exception.Message)));
    }
}