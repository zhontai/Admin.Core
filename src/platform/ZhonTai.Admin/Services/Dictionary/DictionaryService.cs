using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Dictionary;
using ZhonTai.Admin.Services.Dictionary.Dto;
using ZhonTai.Admin.Domain.Dictionary.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Consts;
using System.Linq;

namespace ZhonTai.Admin.Services.Dictionary;

/// <summary>
/// 数据字典服务
/// </summary>
[Order(60)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class DictionaryService : BaseService, IDictionaryService, IDynamicApi
{
    private readonly IDictionaryRepository _dictionaryRepository;

    public DictionaryService(IDictionaryRepository dictionaryRepository)
    {
        _dictionaryRepository = dictionaryRepository;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DictionaryGetOutput> GetAsync(long id)
    {
        var result = await _dictionaryRepository.GetAsync<DictionaryGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<DictionaryListOutput>> GetPageAsync(PageInput<DictionaryGetPageDto> input)
    {
        var key = input.Filter?.Name;
        var dictionaryTypeId = input.Filter?.DictionaryTypeId;
        var list = await _dictionaryRepository.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(dictionaryTypeId.HasValue && dictionaryTypeId.Value > 0, a => a.DictionaryTypeId == dictionaryTypeId)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<DictionaryListOutput>();

        var data = new PageOutput<DictionaryListOutput>()
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
    public async Task<long> AddAsync(DictionaryAddInput input)
    {
        var dictionary = Mapper.Map<DictionaryEntity>(input);
        await _dictionaryRepository.InsertAsync(dictionary);
        return dictionary.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(DictionaryUpdateInput input)
    {
        var entity = await _dictionaryRepository.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("数据字典不存在");
        }

        Mapper.Map(input, entity);
        await _dictionaryRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        await _dictionaryRepository.DeleteAsync(m => m.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchDeleteAsync(long[] ids)
    {
        await _dictionaryRepository.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SoftDeleteAsync(long id)
    {
        await _dictionaryRepository.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _dictionaryRepository.SoftDeleteAsync(ids);
    }
}