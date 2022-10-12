namespace ZhonTai.Admin.Core.Entities;

/// <summary>
/// 部门接口
/// </summary>
public interface IOrg
{
    /// <summary>
    /// 创建者部门Id
    /// </summary>
    long? CreatedOrgId { get; set; }
}