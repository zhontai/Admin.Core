using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Dict;
using ZhonTai.Admin.Services.Dict.Dto;
using ZhonTai.Admin.Domain.Dict.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Consts;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace ZhonTai.Admin.Services.Dict;

/// <summary>
/// 数据字典服务
/// </summary>
[Order(60)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class DictService : BaseService, IDictService, IDynamicApi
{
    private readonly IDictRepository _dictRepository;

    public DictService(IDictRepository dictRepository)
    {
        _dictRepository = dictRepository;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DictGetOutput> GetAsync(long id)
    {
        var result = await _dictRepository.GetAsync<DictGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<DictGetPageOutput>> GetPageAsync(PageInput<DictGetPageDto> input)
    {
        var key = input.Filter?.Name;
        var dictTypeId = input.Filter?.DictTypeId;
        var list = await _dictRepository.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(dictTypeId.HasValue && dictTypeId.Value > 0, a => a.DictTypeId == dictTypeId)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .Count(out var total)
        .OrderByDescending(a => a.Sort)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<DictGetPageOutput>();

        var data = new PageOutput<DictGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="codes">字典类型编码列表</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<Dictionary<string, List<DictGetListDto>>> GetListAsync(string[] codes)
    {
        var list = await _dictRepository.Select
        .Where(a => codes.Contains(a.DictType.Code) && a.DictType.Enabled == true && a.Enabled == true)
        .OrderBy(a => a.Sort)
        .ToListAsync(a => new DictGetListDto { DictTypeCode = a.DictType.Code });

        var dicts = new Dictionary<string, List<DictGetListDto>>();
        foreach (var code in codes)
        {
            if (code.NotNull())
                dicts[code] = list.Where(a => a.DictTypeCode == code).ToList();
        }

        return dicts;
    }

    /// <summary>
    /// 根据字典类型名称列表查询字典列表
    /// </summary>
    /// <param name="names">字典类型名称列表</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<Dictionary<string, List<DictGetListDto>>> GetListByNamesAsync(string[] names)
    {
        var list = await _dictRepository.Select
        .Where(a => names.Contains(a.DictType.Name) && a.DictType.Enabled == true && a.Enabled == true)
        .OrderBy(a => a.Sort)
        .ToListAsync(a => new DictGetListDto { DictTypeName = a.DictType.Name });

        var dicts = new Dictionary<string, List<DictGetListDto>>();
        foreach (var name in names)
        {
            if (name.NotNull())
                dicts[name] = list.Where(a => a.DictTypeName == name).ToList();
        }

        return dicts;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(DictAddInput input)
    {
        if (await _dictRepository.Select.AnyAsync(a => a.DictTypeId == input.DictTypeId && a.Name == input.Name))
        {
            throw ResultOutput.Exception($"字典已存在");
        }

        if (input.Code.NotNull() && await _dictRepository.Select.AnyAsync(a => a.DictTypeId == input.DictTypeId && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"字典编码已存在");
        }

        if (input.Code.NotNull() && await _dictRepository.Select.AnyAsync(a => a.DictTypeId == input.DictTypeId && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"字典编码已存在");
        }

        if (input.Value.NotNull() && await _dictRepository.Select.AnyAsync(a => a.DictTypeId == input.DictTypeId && a.Value == input.Value))
        {
            throw ResultOutput.Exception($"字典值已存在");
        }

        var entity = Mapper.Map<DictEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _dictRepository.Select.Where(a => a.DictTypeId == input.DictTypeId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _dictRepository.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(DictUpdateInput input)
    {
        var entity = await _dictRepository.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("字典不存在");
        }

        if (await _dictRepository.Select.AnyAsync(a => a.Id != input.Id && a.DictTypeId == input.DictTypeId && a.Name == input.Name))
        {
            throw ResultOutput.Exception($"字典已存在");
        }

        if (input.Code.NotNull() && await _dictRepository.Select.AnyAsync(a => a.Id != input.Id && a.DictTypeId == input.DictTypeId && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"字典编码已存在");
        }

        if (input.Value.NotNull() && await _dictRepository.Select.AnyAsync(a => a.Id != input.Id && a.DictTypeId == input.DictTypeId && a.Value == input.Value))
        {
            throw ResultOutput.Exception($"字典值已存在");
        }

        Mapper.Map(input, entity);
        await _dictRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        await _dictRepository.DeleteAsync(m => m.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchDeleteAsync(long[] ids)
    {
        await _dictRepository.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SoftDeleteAsync(long id)
    {
        await _dictRepository.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _dictRepository.SoftDeleteAsync(ids);
    }
}