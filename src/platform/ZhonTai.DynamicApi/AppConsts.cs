using System;
using System.Collections.Generic;
using System.Reflection;
using ZhonTai.DynamicApi.Enums;

namespace ZhonTai.DynamicApi;

public static class AppConsts
{
    public static string DefaultHttpVerb { get; set; }

    public static string DefaultAreaName { get; set; } 

    public static string DefaultApiPreFix { get; set; }

    public static List<string> ControllerPostfixes { get; set; }
    public static List<string> ActionPostfixes { get; set; }

    public static List<Type> FormBodyBindingIgnoredTypes { get; set; }

    public static Dictionary<string,string> HttpVerbs { get; set; }

    public static NamingConventionEnum NamingConvention { get; set; } = NamingConventionEnum.KebabCase;

    public static Func<string, string> GetRestFulControllerName { get; set; }

    public static Func<string, string> GetRestFulActionName { get; set; }

    public static Dictionary<Assembly, AssemblyDynamicApiOptions> AssemblyDynamicApiOptions { get; set; }

    public static bool FormatResult { get; set; } = true;

    public static Type FormatResultType { get; set; } = FormatResultContext.FormatResultType;

    static AppConsts()
    {
        HttpVerbs=new Dictionary<string, string>()
        {
            ["add"] = "POST",
            ["create"] = "POST",
            ["insert"] = "POST",
            ["submit"] = "POST",
            ["post"] = "POST",

            ["get"] = "GET",
            ["find"] = "GET",
            ["fetch"] = "GET",
            ["query"] = "GET",

            ["update"] = "PUT",
            ["change"] = "PUT",
            ["put"] = "PUT",
            ["batch"] = "PUT",

            ["delete"] = "DELETE",
            ["soft"] = "DELETE",
            ["remove"] = "DELETE",
            ["clear"] = "DELETE",
        };
    }
}