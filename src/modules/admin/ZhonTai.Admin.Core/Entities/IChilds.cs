namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 子级接口
/// </summary>
public interface IChilds<T>
{
    /// <summary>
    /// 子级列表
    /// </summary>
    List<T> Childs { get; set; }
}