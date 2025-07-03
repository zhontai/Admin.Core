namespace ZhonTai.Admin.Core.Attributes;

/// <summary>
/// 数据库
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class DatabaseAttribute : Attribute
{
    /// <summary>
    /// 数据库名称
    /// </summary>
    public string Name { get; set; }

    public DatabaseAttribute(string name)
    {
        Name = name;
    }
}