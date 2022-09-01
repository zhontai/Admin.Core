using ZhonTai.Admin.Core.Dto;
using System.Threading.Tasks;
using ZhonTai.Admin.Services.RoleGroup.Dto;

namespace ZhonTai.Admin.Services.RoleGroup;

/// <summary>
/// 角色分组接口
/// </summary>
public partial interface IRoleGroupService
{

    Task<IResultOutput> AddAsync(RoleGroupAddInput input);

    Task<IResultOutput> UpdateAsync(RoleGroupUpdateInput input);

    Task<IResultOutput> DeleteAsync(long id);
}