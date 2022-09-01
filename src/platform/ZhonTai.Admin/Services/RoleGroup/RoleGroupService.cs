using System.Threading.Tasks;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.RoleGroup.Dto;
using ZhonTai.Admin.Domain.RoleGroup;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Domain.Role;

namespace ZhonTai.Admin.Services.RoleGroup;

/// <summary>
/// 角色分组服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class RoleGroupService : BaseService, IRoleGroupService, IDynamicApi
{
    private readonly IRoleGroupRepository _RoleGroupRepository;
    private readonly IRoleRepository _roleRepository;
    public RoleGroupService(IRoleGroupRepository RoleGroupRepository, IRoleRepository roleRepository)
    {
        _RoleGroupRepository = RoleGroupRepository;
        _roleRepository = roleRepository;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> AddAsync(RoleGroupAddInput input)
    {
        var RoleGroup = Mapper.Map<RoleGroupEntity>(input);
        var id = (await _RoleGroupRepository.InsertAsync(RoleGroup)).Id;
        return ResultOutput.Result(id > 0);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> UpdateAsync(RoleGroupUpdateInput input)
    {
        if (!(input?.Id > 0))
        {
            return ResultOutput.NotOk();
        }

        var entity = await _RoleGroupRepository.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            return ResultOutput.NotOk("角色分组不存在！");
        }

        Mapper.Map(input, entity);
        await _RoleGroupRepository.UpdateAsync(entity);
        return ResultOutput.Ok();
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Transaction]
    public async Task<IResultOutput> DeleteAsync(long id)
    {
        //判断角色下是否有用户

        //删除角色数据
        await _roleRepository.DeleteAsync(a => a.RoleGroupId == id);

        //删除角色分组
        await _RoleGroupRepository.DeleteAsync(a => a.Id == id);

        return ResultOutput.Ok();
    }
}