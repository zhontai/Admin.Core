using System;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Module.Dev.Api.Core.Consts;

namespace ZhonTai.Module.Dev.Api.Core.Attributes;

/// <summary>
/// 启用事物
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class AppTransactionAttribute : TransactionAttribute
{
    public AppTransactionAttribute() : base(DbKeys.AppDb)
    {
    }
}