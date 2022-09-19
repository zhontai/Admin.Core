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
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Domain.User;

namespace ZhonTai.Admin.Services.Role;

/// <summary>
/// 角色服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class RoleService : BaseService, IRoleService, IDynamicApi
{
    private IRoleRepository _roleRepository => LazyGetRequiredService<IRoleRepository>();
    private IUserRepository _userRepository => LazyGetRequiredService<IUserRepository>();
    private IRepositoryBase<UserRoleEntity> _userRoleRepository => LazyGetRequiredService<IRepositoryBase<UserRoleEntity>>();
    private IRepositoryBase<RolePermissionEntity> _rolePermissionRepository => LazyGetRequiredService<IRepositoryBase<RolePermissionEntity>>();

    public RoleService()
    {
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
        .OrderBy(a => new {a.ParentId, a.Sort})
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
    /// 查询角色用户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> GetRoleUserListAsync([FromQuery] UserGetRoleUserListInput input)
    {
        var list = await _userRepository.Select.From<UserRoleEntity>()
            .InnerJoin(a => a.t2.UserId == a.t1.Id)
            .Where(a => a.t2.RoleId == input.RoleId)
            .WhereIf(input.Name.NotNull(), a => a.t1.Name.Contains(input.Name))
            .OrderByDescending(a => a.t1.Id)
            .ToListAsync<UserGetRoleUserListOutput>();

        return ResultOutput.Ok(list);
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> AddAsync(RoleAddInput input)
    {
        if(await _roleRepository.Select.AnyAsync(a=>a.ParentId == input.ParentId && a.Name == input.Name))
        {
            return ResultOutput.NotOk($"此{(input.ParentId == 0 ? "分组":"角色")}已存在");
        }

        if (input.Code.NotNull() && await _roleRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
        {
            return ResultOutput.NotOk($"此{(input.ParentId == 0 ? "分组" : "角色")}编码已存在");
        }

        var entity = Mapper.Map<RoleEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _roleRepository.Select.Where(a=>a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
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
    [Transaction]
    public async Task<IResultOutput> DeleteAsync(long id)
    {
        await _userRoleRepository.DeleteAsync(a => a.UserId == id);
        await _rolePermissionRepository.DeleteAsync(a => a.RoleId == id);
        await _roleRepository.DeleteAsync(m => m.Id == id);

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [Transaction]
    public async Task<IResultOutput> BatchDeleteAsync(long[] ids)
    {
        await _userRoleRepository.DeleteAsync(a => ids.Contains(a.RoleId));
        await _rolePermissionRepository.DeleteAsync(a => ids.Contains(a.RoleId));
        await _roleRepository.DeleteAsync(a => ids.Contains(a.Id));

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
        await _userRoleRepository.DeleteAsync(a => a.RoleId == id);
        await _rolePermissionRepository.DeleteAsync(a => a.RoleId == id);
        await _roleRepository.SoftDeleteAsync(id);

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [Transaction]
    public async Task<IResultOutput> BatchSoftDeleteAsync(long[] ids)
    {
        await _userRoleRepository.DeleteAsync(a => ids.Contains(a.RoleId));
        await _rolePermissionRepository.DeleteAsync(a => ids.Contains(a.RoleId));
        await _roleRepository.SoftDeleteAsync(ids);

        return ResultOutput.Ok();
    }
}