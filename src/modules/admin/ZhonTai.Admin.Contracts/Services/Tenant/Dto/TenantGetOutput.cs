using System.Text.Json.Serialization;
using ZhonTai.Admin.Domain.Pkg;

namespace ZhonTai.Admin.Services.Tenant.Dto;

public class TenantGetOutput : TenantUpdateInput
{
    /// <summary>
    /// 套餐列表
    /// </summary>
    [JsonIgnore]
    public ICollection<PkgEntity> Pkgs { get; set; }

    /// <summary>
    /// 套餐Id列表
    /// </summary>
    public override long[] PkgIds => Pkgs?.Select(a => a.Id)?.ToArray();
}