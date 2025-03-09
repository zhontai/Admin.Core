namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 添加接口
/// </summary>
public interface IEntityAdd
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
    /// 创建者姓名
    /// </summary>
    string CreatedUserRealName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    DateTime? CreatedTime { get; set; }
}