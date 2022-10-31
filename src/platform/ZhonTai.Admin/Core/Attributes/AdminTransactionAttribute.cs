using System;
using ZhonTai.Admin.Core.Consts;

namespace ZhonTai.Admin.Core.Attributes;

/// <summary>
/// 启用权限库事物
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class AdminTransactionAttribute : TransactionAttribute
{
    public AdminTransactionAttribute():base(DbKeys.AppDb)
    {
    }
}