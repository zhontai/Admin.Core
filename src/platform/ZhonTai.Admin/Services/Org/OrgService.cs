using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.Org.Input;
using ZhonTai.Admin.Services.Org.Output;
using System.Threading.Tasks;
using ZhonTai.Admin.Domain.Org;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.DynamicApi;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Services.Org;

/// <summary>
/// 部门服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class OrgService : BaseService, IOrgService, IDynamicApi
{
    private readonly IOrgRepository _orgRepository;

    private IRepositoryBase<RoleOrgEntity> _roleOrgRepository => LazyGetRequiredService<IRepositoryBase<RoleOrgEntity>>();

    public OrgService(IOrgRepository orgRepository)
    {
        _orgRepository = orgRepository;
    }

    /// <summary>
    /// 查询部门
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IResultOutput> GetAsync(long id)
    {
        var result = await _orgRepository.GetAsync<OrgGetOutput>(id);
        return ResultOutput.Ok(result);
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<IResultOutput> GetListAsync(string key)
    {
        var data = await _orgRepository
            .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync<OrgListOutput>();

        return ResultOutput.Ok(data);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> AddAsync(OrgAddInput input)
    {
        if (await _orgRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Name == input.Name))
        {
            return ResultOutput.NotOk($"此部门已存在");
        }

        if (input.Code.NotNull() && await _orgRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
        {
            return ResultOutput.NotOk($"此部门编码已存在");
        }

        var dictionary = Mapper.Map<OrgEntity>(input);
        var id = (await _orgRepository.InsertAsync(dictionary)).Id;
        return ResultOutput.Result(id > 0);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> UpdateAsync(OrgUpdateInput input)
    {
        if (!(input?.Id > 0))
        {
            return ResultOutput.NotOk();
        }

        var entity = await _orgRepository.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            return ResultOutput.NotOk("部门不存在");
        }

        if (input.Id == input.ParentId)
        {
            return ResultOutput.NotOk("上级部门不能是本部门");
        }

        if (await _orgRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Name == input.Name))
        {
            return ResultOutput.NotOk($"此部门已存在");
        }

        if (input.Code.NotNull() && await _orgRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Code == input.Code))
        {
            return ResultOutput.NotOk($"此部门编码已存在");
        }

        var childIdList = await _orgRepository.GetChildIdListAsync(input.Id);
        if (childIdList.Contains(input.ParentId))
        {
            return ResultOutput.NotOk($"上级部门不能是下级部门");
        }

        Mapper.Map(input, entity);
        await _orgRepository.UpdateAsync(entity);
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
        //本部门下是否有员工
        if(await _orgRepository.HasUser(id))
        {
            return ResultOutput.NotOk($"当前部门有员工无法删除");
        }

        var orgIdList = await _orgRepository.GetChildIdListAsync(id);
        //本部门的下级部门下是否有员工
        if (await _orgRepository.HasUser(orgIdList))
        {
            return ResultOutput.NotOk($"本部门的下级部门有员工无法删除");
        }

        //删除部门角色
        await _roleOrgRepository.DeleteAsync(a => orgIdList.Contains(a.OrgId));

        //删除本部门和下级部门
        await _orgRepository.DeleteAsync(a => orgIdList.Contains(a.Id));

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Transaction]
    public async Task<IResultOutput> SoftDeleteAsync(long id)
    {
        //本部门下是否有员工
        if (await _orgRepository.HasUser(id))
        {
            return ResultOutput.NotOk($"当前部门有员工无法删除");
        }

        var orgIdList = await _orgRepository.GetChildIdListAsync(id);
        //本部门的下级部门下是否有员工
        if (await _orgRepository.HasUser(orgIdList))
        {
            return ResultOutput.NotOk($"本部门的下级部门有员工无法删除");
        }

        //删除部门角色
        await _roleOrgRepository.SoftDeleteAsync(a => orgIdList.Contains(a.OrgId));

        //删除本部门和下级部门
        await _orgRepository.SoftDeleteAsync(a => orgIdList.Contains(a.Id));

        return ResultOutput.Ok();
    }
}