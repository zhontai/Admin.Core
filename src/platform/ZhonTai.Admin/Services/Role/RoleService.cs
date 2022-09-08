using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.RolePermission;
using ZhonTai.Admin.Services.Role.Dto;
using ZhonTai.Admin.Domain.Role.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Consts;

namespace ZhonTai.Admin.Services.Role;

/// <summary>
/// 角色服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class RoleService : BaseService, IRoleService, IDynamicApi
{
    private IRoleRepository _roleRepository => LazyGetRequiredService<IRoleRepository>();
    private readonly IRepositoryBase<RolePermissionEntity> _rolePermissionRepository;

    public RoleService(
        //IRoleRepository roleRepository,
        IRepositoryBase<RolePermissionEntity> rolePermissionRepository
    )
    {
        //_roleRepository = roleRepository;
        _rolePermissionRepository = rolePermissionRepository;
    }

    /// <summary>
    /// 查询角色
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IResultOutput> GetAsync(long id)
    {
        var result = await _roleRepository.GetAsync<RoleGetOutput>(id);
        return ResultOutput.Ok(result);
    }

    /// <summary>
    /// 查询角色列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> GetListAsync([FromQuery]RoleGetListInput input)
    {
        var list = await _roleRepository.Select
        .WhereIf(input.Name.NotNull(), a => a.Name.Contains(input.Name))
        .OrderByDescending(true, c => c.Id)
        .ToListAsync<RoleGetListOutput>();

        return ResultOutput.Ok(list);
    }

    /// <summary>
    /// 查询角色列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IResultOutput> GetPageAsync(PageInput<RoleGetPageDto> input)
    {
        var key = input.Filter?.Name;

        var list = await _roleRepository.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<RoleGetPageOutput>();

        var data = new PageOutput<RoleGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return ResultOutput.Ok(data);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> AddAsync(RoleAddInput input)
    {
        var entity = Mapper.Map<RoleEntity>(input);
        var id = (await _roleRepository.InsertAsync(entity)).Id;

        return ResultOutput.Result(id > 0);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> UpdateAsync(RoleUpdateInput input)
    {
        if (!(input?.Id > 0))
        {
            return ResultOutput.NotOk();
        }

        var entity = await _roleRepository.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            return ResultOutput.NotOk("角色不存在！");
        }

        Mapper.Map(input, entity);
        await _roleRepository.UpdateAsync(entity);
        return ResultOutput.Ok();
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IResultOutput> DeleteAsync(long id)
    {
        var result = false;
        if (id > 0)
        {
            result = (await _roleRepository.DeleteAsync(m => m.Id == id)) > 0;
        }

        return ResultOutput.Result(result);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IResultOutput> SoftDeleteAsync(long id)
    {
        var result = await _roleRepository.SoftDeleteAsync(id);
        await _rolePermissionRepository.DeleteAsync(a => a.RoleId == id);

        return ResultOutput.Result(result);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<IResultOutput> BatchSoftDeleteAsync(long[] ids)
    {
        var result = await _roleRepository.SoftDeleteAsync(ids);
        await _rolePermissionRepository.DeleteAsync(a => ids.Contains(a.RoleId));

        return ResultOutput.Result(result);
    }
}