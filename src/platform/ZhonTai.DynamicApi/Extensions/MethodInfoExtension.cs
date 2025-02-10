using System.Reflection;

namespace ZhonTai.DynamicApi;

public static class MethodInfoExtension
{
    public static bool IsAsync(this MethodInfo method)
    {
        return method.ReturnType == typeof(Task)
            || (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>));
    }

    internal static Type GetReturnType(this MethodInfo method)
    {
        var isAsync = method.IsAsync();
        var returnType = method.ReturnType;
        return isAsync ? (returnType.GenericTypeArguments.FirstOrDefault() ?? typeof(void)) : returnType;
    }
}