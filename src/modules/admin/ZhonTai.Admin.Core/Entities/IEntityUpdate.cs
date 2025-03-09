namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 修改接口
/// </summary>
public interface IEntityUpdate
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
    /// 修改者姓名
    /// </summary>
    string ModifiedUserRealName { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    DateTime? ModifiedTime { get; set; }
}