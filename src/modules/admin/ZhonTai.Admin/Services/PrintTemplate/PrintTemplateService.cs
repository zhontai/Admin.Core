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
    private readonly AdminRepositoryBase<PrintTemplateEntity> _printTemplateRep;
    private readonly AdminRepositoryBase<UserEntity> _userRep;
    private readonly AdminLocalizer _adminLocalizer;

    public PrintTemplateService(
        AdminRepositoryBase<PrintTemplateEntity> printTemplateRep,
        AdminLocalizer adminLocalizer
    )
    {
        _printTemplateRep = printTemplateRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PrintTemplateGetOutput> GetAsync(long id)
    {
        var output = await _printTemplateRep.Select
        .WhereDynamic(id)
        .ToOneAsync<PrintTemplateGetOutput>();

        return output;
    }

    /// <summary>
    /// 查询修改模板
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PrintTemplateGetUpdateTemplateOutput> GetUpdateTemplateAsync(long id)
    {
        var output = await _printTemplateRep.Select
        .WhereDynamic(id)
        .ToOneAsync<PrintTemplateGetUpdateTemplateOutput>();

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
        var list = await _printTemplateRep.Select
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
        if (await _printTemplateRep.Select.AnyAsync(a => a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["打印模板名称已存在"]);
        }

        if (input.Code.NotNull() && await _printTemplateRep.Select.AnyAsync(a => a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["打印模板编码已存在"]);
        }

        var entity = Mapper.Map<PrintTemplateEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _printTemplateRep.Select.MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _printTemplateRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(PrintTemplateUpdateInput input)
    {
        if (await _printTemplateRep.Select.AnyAsync(a => a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["打印模板名称已存在"]);
        }

        if (input.Code.NotNull() && await _printTemplateRep.Select.AnyAsync(a => a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["打印模板编码已存在"]);
        }

        var entity = await _printTemplateRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["打印模板不存在"]);
        }

        if(entity.Version != input.Version)
        {
            throw ResultOutput.Exception(_adminLocalizer["打印模板已被修改，请刷新后重试"]);
        }

        Mapper.Map(input, entity);
        await _printTemplateRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateTemplateAsync(PrintTemplateUpdateTemplateInput input)
    {
        var entity = await _printTemplateRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["打印模板不存在"]);
        }

        if (entity.Version != input.Version)
        {
            throw ResultOutput.Exception(_adminLocalizer["打印模板已被修改，请刷新后重试"]);
        }

        entity.Template = input.Template;
        entity.PrintData = input.PrintData;
        await _printTemplateRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 设置启用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetEnableAsync(PrintTemplateSetEnableInput input)
    {
        var printTemplateRep = _printTemplateRep;
        using var _ = printTemplateRep.DataFilter.DisableAll();

        var entity = await printTemplateRep.GetAsync(input.PrintTemplateId);
        entity.Enabled = input.Enabled;
        await printTemplateRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        await _printTemplateRep.DeleteAsync(id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        await _printTemplateRep.Where(a => ids.Contains(a.Id)).ToDelete().ExecuteAffrowsAsync();
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _printTemplateRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _printTemplateRep.SoftDeleteAsync(a => ids.Contains(a.Id));
    }
}