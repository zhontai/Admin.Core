using Castle.DynamicProxy;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ZhonTai.Admin.Core.Aop;

public class AopHelper
{
    public static async Task<T> ExecuteGenericMethod<T>(Task<T> returnValue, Action<T> callBackAction, Action<Exception> exceptionAction, Action finallyAction)
    {
        try
        {
            var result = await returnValue;
            callBackAction?.Invoke(result);
            return result;
        }
        catch (Exception ex)
        {
            exceptionAction?.Invoke(ex);
            return default;
        }
        finally
        {
            finallyAction?.Invoke();
        }
    }

    public static object CallGenericMethod(IInvocation invocation, Action<object> callBackAction, Action<Exception> exceptionAction, Action finallyAction)
    {
        return typeof(AopHelper)
        .GetMethod("ExecuteGenericMethod", BindingFlags.Public | BindingFlags.Static)
        .MakeGenericMethod(invocation.Method.ReturnType.GenericTypeArguments[0])
        .Invoke(null, new object[] { invocation.ReturnValue, callBackAction, exceptionAction, finallyAction });
    }
}