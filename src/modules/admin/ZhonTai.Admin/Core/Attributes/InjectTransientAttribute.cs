using System;

namespace ZhonTai.Admin.Core.Attributes;

/// <summary>
/// 瞬时注入
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class InjectTransientAttribute : Attribute
{
}