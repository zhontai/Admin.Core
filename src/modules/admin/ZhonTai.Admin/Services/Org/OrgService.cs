using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.Org.Input;
using ZhonTai.Admin.Services.Org.Output;
using System.Threading.Tasks;
using ZhonTai.Admin.Domain.Org;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.DynamicApi;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Domain.RoleOrg;
using ZhonTai.Admin.Domain.UserOrg;
using System.Collections.Generic;
using System.Linq;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Resources;

namespace ZhonTai.Admin.Services.Org;

/// <summary>
/// 部门服务
/// </summary>
[Order(30)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class OrgService : BaseService, IOrgService, IDynamicApi
{
    private readonly IOrgRepository _orgRep;
    private readonly IUserOrgRepository _userOrgRep;
    private readonly IRoleOrgRepository _roleOrgRep;
    private readonly AdminLocalizer _localizer;

    public OrgService(
        IOrgRepository orgRep,
        IUserOrgRepository userOrgRep,
        IRoleOrgRepository roleOrgRep,
        AdminLocalizer localizer
    )
    {
        _orgRep = orgRep;
        _userOrgRep = userOrgRep;
        _roleOrgRep = roleOrgRep;
        _localizer = localizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrgGetOutput> GetAsync(long id)
    {
        var result = await _orgRep.GetAsync<OrgGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<List<OrgListOutput>> GetListAsync(string key)
    {
        var dataPermission = User.DataPermission;

        var data = await _orgRep.Select
            .WhereIf(dataPermission.OrgIds.Count > 0, a => dataPermission.OrgIds.Contains(a.Id))
            .WhereIf(dataPermission.DataScope == DataScope.Self, a => a.CreatedUserId == User.Id)
            .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
            .ToListAsync<OrgListOutput>();

        return data?.Count > 0 ? data.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort).ToList() : data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(OrgAddInput input)
    {
        if(input.ParentId == 0)
        {
            throw ResultOutput.Exception(_localizer["请选择上级部门"]);
        }

        if (await _orgRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_localizer["此部门已存在"]);
        }

        if (input.Code.NotNull() && await _orgRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_localizer["此部门编码已存在"]);
        }

        var entity = Mapper.Map<OrgEntity>(input);

        if (entity.Sort == 0)
        {
            var sort = await _orgRep.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _orgRep.InsertAsync(entity);
        await Cache.DelByPatternAsync(CacheKeys.DataPermission + "*");

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(OrgUpdateInput input)
    {
        if (input.ParentId == 0)
        {
            throw ResultOutput.Exception(_localizer["请选择上级部门"]);
        }

        var entity = await _orgRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_localizer["部门不存在"]);
        }

        if (input.Id == input.ParentId)
        {
            throw ResultOutput.Exception(_localizer["上级部门不能是本部门"]);
        }

        if (await _orgRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_localizer["此部门已存在"]);
        }

        if (input.Code.NotNull() && await _orgRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_localizer["此部门编码已存在"]);
        }

        var childIdList = await _orgRep.GetChildIdListAsync(input.Id);
        if (childIdList.Contains(input.ParentId))
        {
            throw ResultOutput.Exception(_localizer["上级部门不能是下级部门"]);
        }

        Mapper.Map(input, entity);
        await _orgRep.UpdateAsync(entity);

        await Cache.DelByPatternAsync(CacheKeys.DataPermission + "*");
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public async Task DeleteAsync(long id)
    {
        //本部门下是否有员工
        if(await _userOrgRep.HasUser(id))
        {
            throw ResultOutput.Exception(_localizer["当前部门有员工无法删除"]);
        }

        var orgIdList = await _orgRep.GetChildIdListAsync(id);
        //本部门的下级部门下是否有员工
        if (await _userOrgRep.HasUser(orgIdList))
        {
            throw ResultOutput.Exception(_localizer["本部门的下级部门有员工无法删除"]);
        }

        //删除部门角色
        await _roleOrgRep.DeleteAsync(a => orgIdList.Contains(a.OrgId));

        //删除本部门和下级部门
        await _orgRep.DeleteAsync(a => orgIdList.Contains(a.Id));

        await Cache.DelByPatternAsync(CacheKeys.DataPermission + "*");
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public async Task SoftDeleteAsync(long id)
    {
        //本部门下是否有员工
        if (await _userOrgRep.HasUser(id))
        {
            throw ResultOutput.Exception(_localizer["当前部门有员工无法删除"]);
        }

        var orgIdList = await _orgRep.GetChildIdListAsync(id);
        //本部门的下级部门下是否有员工
        if (await _userOrgRep.HasUser(orgIdList))
        {
            throw ResultOutput.Exception(_localizer["本部门的下级部门有员工无法删除"]);
        }

        //删除部门角色
        await _roleOrgRep.SoftDeleteAsync(a => orgIdList.Contains(a.OrgId));

        //删除本部门和下级部门
        await _orgRep.SoftDeleteAsync(a => orgIdList.Contains(a.Id));

        await Cache.DelByPatternAsync(CacheKeys.DataPermission + "*");
    }
}