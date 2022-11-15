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

namespace ZhonTai.Admin.Services.Org;

/// <summary>
/// 部门服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class OrgService : BaseService, IOrgService, IDynamicApi
{
    private IOrgRepository _orgRepository => LazyGetRequiredService<IOrgRepository>();
    private IUserOrgRepository _userOrgRepository => LazyGetRequiredService<IUserOrgRepository>();
    private IRoleOrgRepository _roleOrgRepository => LazyGetRequiredService<IRoleOrgRepository>();

    public OrgService()
    {
    }

    /// <summary>
    /// 查询部门
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrgGetOutput> GetAsync(long id)
    {
        var result = await _orgRepository.GetAsync<OrgGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<List<OrgListOutput>> GetListAsync(string key)
    {
        var data = await _orgRepository
            .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync<OrgListOutput>();

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(OrgAddInput input)
    {
        if (await _orgRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Name == input.Name))
        {
            throw ResultOutput.Exception($"此部门已存在");
        }

        if (input.Code.NotNull() && await _orgRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"此部门编码已存在");
        }

        var entity = Mapper.Map<OrgEntity>(input);

        if (entity.Sort == 0)
        {
            var sort = await _orgRepository.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _orgRepository.InsertAsync(entity);
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
        var entity = await _orgRepository.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("部门不存在");
        }

        if (input.Id == input.ParentId)
        {
            throw ResultOutput.Exception("上级部门不能是本部门");
        }

        if (await _orgRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception($"此部门已存在");
        }

        if (input.Code.NotNull() && await _orgRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"此部门编码已存在");
        }

        var childIdList = await _orgRepository.GetChildIdListAsync(input.Id);
        if (childIdList.Contains(input.ParentId))
        {
            throw ResultOutput.Exception($"上级部门不能是下级部门");
        }

        Mapper.Map(input, entity);
        await _orgRepository.UpdateAsync(entity);

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
        if(await _userOrgRepository.HasUser(id))
        {
            throw ResultOutput.Exception($"当前部门有员工无法删除");
        }

        var orgIdList = await _orgRepository.GetChildIdListAsync(id);
        //本部门的下级部门下是否有员工
        if (await _userOrgRepository.HasUser(orgIdList))
        {
            throw ResultOutput.Exception($"本部门的下级部门有员工无法删除");
        }

        //删除部门角色
        await _roleOrgRepository.DeleteAsync(a => orgIdList.Contains(a.OrgId));

        //删除本部门和下级部门
        await _orgRepository.DeleteAsync(a => orgIdList.Contains(a.Id));

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
        if (await _userOrgRepository.HasUser(id))
        {
            throw ResultOutput.Exception($"当前部门有员工无法删除");
        }

        var orgIdList = await _orgRepository.GetChildIdListAsync(id);
        //本部门的下级部门下是否有员工
        if (await _userOrgRepository.HasUser(orgIdList))
        {
            throw ResultOutput.Exception($"本部门的下级部门有员工无法删除");
        }

        //删除部门角色
        await _roleOrgRepository.SoftDeleteAsync(a => orgIdList.Contains(a.OrgId));

        //删除本部门和下级部门
        await _orgRepository.SoftDeleteAsync(a => orgIdList.Contains(a.Id));

        await Cache.DelByPatternAsync(CacheKeys.DataPermission + "*");
    }
}