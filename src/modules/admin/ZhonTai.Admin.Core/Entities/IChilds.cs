using System.Collections.Generic;

namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 子级接口
/// </summary>
public interface IChilds<T>
{
    List<T> Childs { get; set; }
}