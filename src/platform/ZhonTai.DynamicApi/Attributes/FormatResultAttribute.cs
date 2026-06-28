using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ZhonTai.DynamicApi.Attributes;

[Serializable]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class FormatResultAttribute : ProducesResponseTypeAttribute
{
    /// <summary>
    /// The type of the result when the action returns void.
    /// </summary>
    private static readonly Type _voidResultType = AppConsts.FormatResultType.MakeGenericType(typeof(object));

    public FormatResultAttribute(int statusCode) : base(statusCode)
    {
    }

    public FormatResultAttribute(Type type) : base(type, StatusCodes.Status200OK)
    {
        FormatType(type);
    }

    public FormatResultAttribute(Type type, int statusCode) : base(type, statusCode)
    {
        FormatType(type);
    }

    private void FormatType(Type type)
    {
        if (type != null && type != typeof(void))
        {
            Type = AppConsts.FormatResultType.MakeGenericType(type);
        }
        else
        {
            Type = _voidResultType;
        }
    }
}