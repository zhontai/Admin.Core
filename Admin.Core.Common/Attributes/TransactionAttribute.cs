using System;

namespace Admin.Core.Common
{
    /// <summary>
    /// 启用事物
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class TransactionAttribute : Attribute
    {

    }
}
