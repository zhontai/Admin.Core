using System;

namespace ZhonTai.DynamicApi.Attributes;

/// <summary>
/// 不格式化结果数据
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class NonFormatResultAttribute : Attribute
{
}