namespace ZhonTai.Admin.Core.Entities;

public interface ITenant
{
    /// <summary>
    /// 租户Id
    /// </summary>
    long? TenantId { get; set; }
}