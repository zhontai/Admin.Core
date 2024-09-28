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
using System;
using ZhonTai.Admin.Repositories;
using ZhonTai.Admin.Resources;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Magicodes.ExporterAndImporter.Core.Models;
using ZhonTai.Admin.Domain.DictType;
using Mapster;
using ZhonTai.Admin.Core.Helpers;
using ZhonTai.Admin.Core.Db;

namespace ZhonTai.Admin.Services.Dict;

/// <summary>
/// 数据字典服务
/// </summary>
[Order(60)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class DictService : BaseService, IDictService, IDynamicApi
{
    private readonly AdminRepositoryBase<DictEntity> _dictRep;
    private readonly AdminRepositoryBase<DictTypeEntity> _dictTypeRep;
    private readonly AdminLocalizer _adminLocalizer;
    private readonly IEHelper _iEHelper;

    public DictService(AdminRepositoryBase<DictEntity> dictRep,
        AdminRepositoryBase<DictTypeEntity> dictTypeRep,
        AdminLocalizer adminLocalizer,
        IEHelper iEHelper)
    {
        _dictRep = dictRep;
        _dictTypeRep = dictTypeRep;
        _adminLocalizer = adminLocalizer;
        _iEHelper = iEHelper;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DictGetOutput> GetAsync(long id)
    {
        var result = await _dictRep.GetAsync<DictGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<DictGetPageOutput>> GetPageAsync(PageInput<DictGetPageInput> input)
    {
        var key = input.Filter?.Name;
        var dictTypeId = input.Filter?.DictTypeId;

        var select = _dictRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(dictTypeId.HasValue && dictTypeId.Value > 0, a => a.DictTypeId == dictTypeId)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .Count(out var total);

        if (input.SortList != null && input.SortList.Count > 0)
        {
            input.SortList.ForEach(sort =>
            {
                select = select.OrderByPropertyNameIf(sort.Order.HasValue, sort.PropName, sort.IsAscending.Value);
            });
        }
        else
        {
            select = select.OrderBy(a => a.Sort);
        }

        var list = await select
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
    /// 通过类型编码查询列表
    /// </summary>
    /// <param name="codes">字典类型编码列表</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<Dictionary<string, List<DictGetListDto>>> GetListAsync(string[] codes)
    {
        var list = await _dictRep.Select
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
    /// 通过类型名称查询列表
    /// </summary>
    /// <param name="names">字典类型名称列表</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<Dictionary<string, List<DictGetListDto>>> GetListByNamesAsync(string[] names)
    {
        var list = await _dictRep.Select
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
    /// 下载导入模板
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [NonFormatResult]
    public async Task<ActionResult> DownloadTemplateAsync()
    {
        var fileName = _adminLocalizer["数据字典模板{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")];
        return await _iEHelper.DownloadTemplateAsync(new DictImport(), fileName);
    }

    /// <summary>
    /// 下载错误标记文件
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    [HttpPost]
    [NonFormatResult]
    public async Task<ActionResult> DownloadErrorMarkAsync(string fileId, string fileName)
    {
        if (fileName.IsNull())
        {
            fileName = _adminLocalizer["数据字典错误标记{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")];
        }
        return await _iEHelper.DownloadErrorMarkAsync(fileId, fileName);
    }

    /// <summary>
    /// 导出数据
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [NonFormatResult]
    public async Task<ActionResult> ExportDataAsync(ExportInput input)
    {
        using var _ = _dictRep.DataFilter.DisableAll();

        var select = _dictRep.Select;
        if (input.SortList != null && input.SortList.Count > 0)
        {
            select = select.SortList(input.SortList);
        }
        else
        {
            select = select.OrderBy(a => a.DictType.Sort).OrderBy(a => a.Sort);
        }

        //查询数据
        var dataList = await select.WhereDynamicFilter(input.DynamicFilter).ToListAsync(a => new DictExport { DictTypeName = a.DictType.Name });

        var dictTypeName = dataList.Count > 0 ? dataList[0].DictTypeName : string.Empty;

        //导出数据
        var fileName = input.FileName.NotNull() ? input.FileName : _adminLocalizer["数据字典-{0}列表{1}.xlsx", dictTypeName, DateTime.Now.ToString("yyyyMMddHHmmss")];

        return await _iEHelper.ExportDataAsync(dataList, fileName, dictTypeName);
    }

    /// <summary>
    /// 导入数据
    /// </summary>
    /// <param name="file"></param>
    /// <param name="duplicateAction"></param>
    /// <param name="fileId"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ImportOutput> ImportDataAsync([Required] IFormFile file, int duplicateAction, string fileId)
    {
        var importResult = await _iEHelper.ImportDataAsync<DictImport>(file, fileId, async (importResult) =>
        {
            //检查数据
            var importDataList = importResult.Data;
            var importDictTypeNameList = importDataList.Select(a => a.DictTypeName).Distinct().ToList();
            var dictTypeList = await _dictTypeRep.Where(a => importDictTypeNameList.Contains(a.Name)).ToListAsync(a => new { a.Id, a.Name });
            var dictTypeIdList = dictTypeList.Select(a => a.Id).Distinct().ToList();
            var dictList = await _dictRep.Where(a => dictTypeIdList.Contains(a.DictTypeId)).ToListAsync(a => new { a.Id, a.DictTypeId, a.Name, a.Code, a.Value });

            if (importResult.RowErrors == null)
            {
                importResult.RowErrors = new List<DataRowErrorInfo>();
            }
            var errorList = importResult.RowErrors;

            foreach (var importData in importDataList)
            {
                var rowIndex = importDataList.ToList().FindIndex(o => o.Equals(importData)) + 2;
                importData.DictTypeId = dictTypeList.Where(a => a.Name == importData.DictTypeName).Select(a => a.Id).FirstOrDefault();

                if (importData.DictTypeId > 0)
                {
                    importData.Id = dictList.Where(a => a.DictTypeId == importData.DictTypeId && a.Name == importData.Name).Select(a => a.Id).FirstOrDefault();

                    if (importData.Id > 0)
                    {
                        if (duplicateAction == 1)
                        {
                            if (importData.Code.NotNull() && dictList.Where(a => a.Id != importData.Id && a.DictTypeId == importData.DictTypeId && a.Code == importData.Code).Any())
                            {
                                var errorInfo = new DataRowErrorInfo()
                                {
                                    RowIndex = rowIndex,
                                };
                                errorInfo.FieldErrors.Add("字典编码", importData.Code + "已存在");
                                errorList.Add(errorInfo);
                            }

                            if (importData.Value.NotNull() && dictList.Where(a => a.Id != importData.Id && a.DictTypeId == importData.DictTypeId && a.Value == importData.Value).Any())
                            {
                                var errorInfo = new DataRowErrorInfo()
                                {
                                    RowIndex = rowIndex,
                                };
                                errorInfo.FieldErrors.Add("字典值", importData.Value + "已存在");
                                errorList.Add(errorInfo);
                            }
                        }
                    }
                    else
                    {
                        if (importData.Code.NotNull() && dictList.Where(a => a.DictTypeId == importData.DictTypeId && a.Code == importData.Code).Any())
                        {
                            var errorInfo = new DataRowErrorInfo()
                            {
                                RowIndex = rowIndex,
                            };
                            errorInfo.FieldErrors.Add("字典编码", importData.Code + "已存在");
                            errorList.Add(errorInfo);
                        }

                        if (importData.Value.NotNull() && dictList.Where(a => a.DictTypeId == importData.DictTypeId && a.Value == importData.Value).Any())
                        {
                            var errorInfo = new DataRowErrorInfo()
                            {
                                RowIndex = rowIndex,
                            };
                            errorInfo.FieldErrors.Add("字典值", importData.Value + "已存在");
                            errorList.Add(errorInfo);
                        }
                    }
                }
                else
                {
                    var errorInfo = new DataRowErrorInfo()
                    {
                        RowIndex = rowIndex,
                    };
                    errorInfo.FieldErrors.Add("字典分类", importData.DictTypeName + "不存在");
                    errorList.Add(errorInfo);
                }
            }

            return importResult;
        });

        var importDataList = importResult.Data;
        var output = new ImportOutput()
        {
            Total = importDataList.Count
        };
        if (output.Total > 0)
        {
            //新增
            var insetImportDataList = importDataList.Where(a=>a.Id == 0).ToList();
            var insetDataList = insetImportDataList.Adapt<List<DictEntity>>();
            output.InsertCount = insetDataList.Count;
            await _dictRep.InsertAsync(insetDataList);

            //修改
            var updateImportDataList = importDataList.Where(a => a.Id > 0).ToList();
            if (duplicateAction == 1 && updateImportDataList?.Count > 0)
            {
                var updateImportDataIds = updateImportDataList.Select(e => e.Id).ToList();
                var dbDataList = await _dictRep.Where(a => updateImportDataIds.Contains(a.Id)).ToListAsync();
                foreach (var dbData in dbDataList)
                {
                    var data = updateImportDataList.Where(a => a.Id == dbData.Id).First();
                    data.Adapt(dbData);
                }
                output.UpdateCount = updateImportDataList.Count;
                await _dictRep.UpdateAsync(dbDataList);
            }
        }

        return output;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(DictAddInput input)
    {
        if (await _dictRep.Select.AnyAsync(a => a.DictTypeId == input.DictTypeId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["字典已存在"]);
        }

        if (input.Code.NotNull() && await _dictRep.Select.AnyAsync(a => a.DictTypeId == input.DictTypeId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["字典编码已存在"]);
        }

        if (input.Value.NotNull() && await _dictRep.Select.AnyAsync(a => a.DictTypeId == input.DictTypeId && a.Value == input.Value))
        {
            throw ResultOutput.Exception(_adminLocalizer["字典值已存在"]);
        }

        var entity = Mapper.Map<DictEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _dictRep.Select.Where(a => a.DictTypeId == input.DictTypeId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _dictRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(DictUpdateInput input)
    {
        var entity = await _dictRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["字典不存在"]);
        }

        if (await _dictRep.Select.AnyAsync(a => a.Id != input.Id && a.DictTypeId == input.DictTypeId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["字典已存在"]);
        }

        if (input.Code.NotNull() && await _dictRep.Select.AnyAsync(a => a.Id != input.Id && a.DictTypeId == input.DictTypeId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["字典编码已存在"]);
        }

        if (input.Value.NotNull() && await _dictRep.Select.AnyAsync(a => a.Id != input.Id && a.DictTypeId == input.DictTypeId && a.Value == input.Value))
        {
            throw ResultOutput.Exception(_adminLocalizer["字典值已存在"]);
        }

        Mapper.Map(input, entity);
        await _dictRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        await _dictRep.DeleteAsync(m => m.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchDeleteAsync(long[] ids)
    {
        await _dictRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SoftDeleteAsync(long id)
    {
        await _dictRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _dictRep.SoftDeleteAsync(ids);
    }
}