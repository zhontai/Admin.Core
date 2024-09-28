using System;

namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 添加接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IEntityAdd<TKey> where TKey : struct
{
    /// <summary>
    /// 创建者用户Id
    /// </summary>
    long? CreatedUserId { get; set; }
    /// <summary>
    /// 创建者
    /// </summary>
    string CreatedUserName { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    DateTime? CreatedTime { get; set; }
}