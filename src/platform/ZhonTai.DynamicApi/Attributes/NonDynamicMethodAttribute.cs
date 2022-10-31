using System;

namespace ZhonTai.DynamicApi.Attributes;

[Serializable]
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
public class NonDynamicMethodAttribute : Attribute
{

}
