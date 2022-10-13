namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 数据权限接口
/// </summary>
public interface IData
{
    /// <summary>
    /// 拥有者Id
    /// </summary>
    long? OwnerId { get; set; }

    /// <summary>
    /// 创建者部门Id
    /// </summary>
    long? CreatedOrgId { get; set; }
}