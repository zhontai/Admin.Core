using System;

namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 修改接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IEntityUpdate<TKey> where TKey : struct
{
    /// <summary>
    /// 修改者Id
    /// </summary>
    long? ModifiedUserId { get; set; }
    /// <summary>
    /// 修改者
    /// </summary>
    string ModifiedUserName { get; set; }
    /// <summary>
    /// 修改时间
    /// </summary>
    DateTime? ModifiedTime { get; set; }
}