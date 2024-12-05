using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.MsgType;
using ZhonTai.Admin.Services.MsgType.Dto;
using ZhonTai.Admin.Resources;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Repositories;

namespace ZhonTai.Admin.Services.MsgType;

/// <summary>
/// 消息分类服务
/// </summary>
[Order(20)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class MsgTypeService : BaseService, IDynamicApi
{
    private readonly AdminRepositoryBase<MsgTypeEntity> _msgTypeRep;
    private readonly AdminLocalizer _adminLocalizer;

    public MsgTypeService(AdminRepositoryBase<MsgTypeEntity> msgTypeRep, AdminLocalizer adminLocalizer)
    {
        _msgTypeRep = msgTypeRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<MsgTypeGetOutput> GetAsync(long id)
    {
        var output = await _msgTypeRep.Select
        .WhereDynamic(id)
        .ToOneAsync<MsgTypeGetOutput>();

        return output;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<MsgTypeGetListOutput>> GetListAsync([FromQuery]MsgTypeGetListInput input)
    {
        var list = await _msgTypeRep.Select
        .WhereIf(input.Name.NotNull(), a => a.Name.Contains(input.Name))
        .OrderBy(a => new {a.ParentId, a.Sort})
        .ToListAsync<MsgTypeGetListOutput>();

        return list;
    }
        
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(MsgTypeAddInput input)
    {
        if (await _msgTypeRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["此分类已存在"]);
        }

        if (input.Code.NotNull() && await _msgTypeRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["此{分类编码已存在"]);
        }

        var entity = Mapper.Map<MsgTypeEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _msgTypeRep.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _msgTypeRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(MsgTypeUpdateInput input)
    {
        if (input.Id == input.ParentId) 
        {
            throw ResultOutput.Exception(_adminLocalizer["上级分组不能是自己"]);
        }

        var entity = await _msgTypeRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["分类不存在"]);
        }

        if (await _msgTypeRep.Select.AnyAsync(a => a.Id != input.Id && a.ParentId == input.ParentId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["此分类已存在"]);
        }

        if (input.Code.NotNull() && await _msgTypeRep.Select.AnyAsync(a => a.Id != input.Id && a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["此{分类编码已存在"]);
        }

        Mapper.Map(input, entity);
        await _msgTypeRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 获得本级和下级Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<List<long>> GetChildIdListAsync(long id)
    {
        return await _msgTypeRep.Select
        .Where(a => a.Id == id)
        .AsTreeCte()
        .ToListAsync(a => a.Id);
    }

    /// <summary>
    /// 获得本级和下级Id
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<List<long>> GetChildIdListAsync(long[] ids)
    {
        return await _msgTypeRep.Select
        .Where(a => ids.Contains(a.Id))
        .AsTreeCte()
        .ToListAsync(a => a.Id);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var msgTypeIdList = await GetChildIdListAsync(id);
        await _msgTypeRep.DeleteAsync(a => msgTypeIdList.Contains(a.Id));
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        var msgTypeIdList = await GetChildIdListAsync(ids);
        await _msgTypeRep.Where(a => msgTypeIdList.Contains(a.Id)).AsTreeCte().ToDelete().ExecuteAffrowsAsync();
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        var msgTypeIdList = await GetChildIdListAsync(id);
        await _msgTypeRep.SoftDeleteRecursiveAsync(a => msgTypeIdList.Contains(a.Id));
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        var msgTypeIdList = await GetChildIdListAsync(ids);
        await _msgTypeRep.SoftDeleteRecursiveAsync(a => msgTypeIdList.Contains(a.Id));
    }
}