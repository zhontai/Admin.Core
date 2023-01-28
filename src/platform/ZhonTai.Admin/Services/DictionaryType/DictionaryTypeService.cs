using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.DictionaryType.Dto;
using ZhonTai.Admin.Domain.DictionaryType;
using ZhonTai.Admin.Domain.Dictionary;
using ZhonTai.Admin.Domain.DictionaryType.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Consts;

namespace ZhonTai.Admin.Services.DictionaryType;

/// <summary>
/// 数据字典类型服务
/// </summary>
[Order(61)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class DictionaryTypeService : BaseService, IDictionaryTypeService, IDynamicApi
{
    private readonly IDictionaryTypeRepository _DictionaryTypeRepository;
    private readonly IDictionaryRepository _dictionaryRepository;
    public DictionaryTypeService(IDictionaryTypeRepository DictionaryTypeRepository, IDictionaryRepository dictionaryRepository)
    {
        _DictionaryTypeRepository = DictionaryTypeRepository;
        _dictionaryRepository = dictionaryRepository;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DictionaryTypeGetOutput> GetAsync(long id)
    {
        var result = await _DictionaryTypeRepository.GetAsync<DictionaryTypeGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<DictionaryTypeListOutput>> GetPageAsync(PageInput<DictionaryTypeGetPageDto> input)
    {
        var key = input.Filter?.Name;

        var list = await _DictionaryTypeRepository.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<DictionaryTypeListOutput>();

        var data = new PageOutput<DictionaryTypeListOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(DictionaryTypeAddInput input)
    {
        var DictionaryType = Mapper.Map<DictionaryTypeEntity>(input);
        await _DictionaryTypeRepository.InsertAsync(DictionaryType);
        return DictionaryType.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(DictionaryTypeUpdateInput input)
    {
        var entity = await _DictionaryTypeRepository.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("数据字典不存在！");
        }

        Mapper.Map(input, entity);
        await _DictionaryTypeRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        //删除字典数据
        await _dictionaryRepository.DeleteAsync(a => a.DictionaryTypeId == id);

        //删除数据字典类型
        await _DictionaryTypeRepository.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        //删除字典数据
        await _dictionaryRepository.DeleteAsync(a => ids.Contains(a.DictionaryTypeId));

        //删除数据字典类型
        await _DictionaryTypeRepository.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _dictionaryRepository.SoftDeleteAsync(a => a.DictionaryTypeId == id);
        await _DictionaryTypeRepository.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _dictionaryRepository.SoftDeleteAsync(a => ids.Contains(a.DictionaryTypeId));
        await _DictionaryTypeRepository.SoftDeleteAsync(ids);
    }
}