using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.PrintTemplate;
using ZhonTai.Admin.Resources;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Repositories;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Services.PrintTemplate.Ouputs;
using ZhonTai.Admin.Services.PrintTemplate.Inputs;

namespace ZhonTai.Admin.Services.PrintTemplate;

/// <summary>
/// 打印模板服务
/// </summary>
[Order(20)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class PrintTemplateService : BaseService, IDynamicApi
{
    private readonly AdminRepositoryBase<PrintTemplateEntity> _msgRep;
    private readonly AdminRepositoryBase<UserEntity> _userRep;
    private readonly AdminLocalizer _adminLocalizer;

    public PrintTemplateService(
        AdminRepositoryBase<PrintTemplateEntity> msgRep,
        AdminLocalizer adminLocalizer
    )
    {
        _msgRep = msgRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PrintTemplateGetOutput> GetAsync(long id)
    {
        var output = await _msgRep.Select
        .WhereDynamic(id)
        .ToOneAsync<PrintTemplateGetOutput>();

        return output;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<PrintTemplateGetPageOutput>> GetPageAsync(PageInput<PrintTemplateGetPageInput> input)
    {
        var list = await _msgRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(input.Filter != null && input.Filter.Name.NotNull(), a => a.Name.Contains(input.Filter.Name))
        .WhereIf(input.Filter != null && input.Filter.Code.NotNull(), a => a.Code.Contains(input.Filter.Code))
        .Count(out var total)
        .OrderByDescending(true, a => a.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync(a => new PrintTemplateGetPageOutput { });

        var data = new PageOutput<PrintTemplateGetPageOutput>()
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
    public async Task<long> AddAsync(PrintTemplateAddInput input)
    {
        var entity = Mapper.Map<PrintTemplateEntity>(input);
        await _msgRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(PrintTemplateUpdateInput input)
    {
        var entity = await _msgRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["打印模板不存在"]);
        }

        if(entity.Version != input.Version)
        {
            throw ResultOutput.Exception(_adminLocalizer["打印模板已被修改，请刷新后重试"]);
        }

        Mapper.Map(input, entity);
        await _msgRep.UpdateAsync(entity);
    }    

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        await _msgRep.DeleteAsync(id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        await _msgRep.Where(a => ids.Contains(a.Id)).ToDelete().ExecuteAffrowsAsync();
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _msgRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _msgRep.SoftDeleteAsync(a => ids.Contains(a.Id));
    }
}