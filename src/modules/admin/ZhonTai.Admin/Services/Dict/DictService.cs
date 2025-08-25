using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Magicodes.ExporterAndImporter.Core.Models;
using Mapster;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Helpers;
using ZhonTai.Admin.Domain.Dict;
using ZhonTai.Admin.Domain.DictType;
using ZhonTai.Admin.Domain.Dict.Dto;
using ZhonTai.Admin.Repositories;
using ZhonTai.Admin.Resources;
using ZhonTai.Admin.Services.Dict.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Yitter.IdGenerator;

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
    [Obsolete($"请使用{nameof(GetAllAsync)}方法替代")]
    [HttpPost]
    public async Task<PageOutput<DictGetPageOutput>> GetPageAsync(PageInput<DictGetPageInput> input)
    {
        var key = input.Filter?.Name;
        var dictTypeId = input.Filter?.DictTypeId;

        var select = _dictRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(dictTypeId.HasValue && dictTypeId.Value > 0, a => a.DictTypeId == dictTypeId)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .Count(out var total)
        .OrderBy(a => a.Parent);

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
    /// 查询列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<DictGetAllOutput>> GetAllAsync(DictGetAllInput input)
    {
        var key = input?.Name;

        var select = _dictRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .Where(a => a.DictTypeId == input.DictTypeId)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .OrderBy(a => a.ParentId);

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

        return await select.ToListAsync<DictGetAllOutput>();
    }

    /// <summary>
    /// 通过类型编码查询列表
    /// </summary>
    /// <param name="codes">字典类型编码列表</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<Dictionary<string, List<DictGetListOutput>>> GetListAsync(string[] codes)
    {
        var list = await _dictRep.Select
        .Where(a => codes.Contains(a.DictType.Code) && a.DictType.Enabled == true && a.Enabled == true)
        .OrderBy(a => a.Sort)
        .ToListAsync(a => new DictGetListOutput { DictTypeCode = a.DictType.Code });

        var dicts = new Dictionary<string, List<DictGetListOutput>>();
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
    public async Task<Dictionary<string, List<DictGetListOutput>>> GetListByNamesAsync(string[] names)
    {
        var list = await _dictRep.Select
        .Where(a => names.Contains(a.DictType.Name) && a.DictType.Enabled == true && a.Enabled == true)
        .OrderBy(a => a.Sort)
        .ToListAsync(a => new DictGetListOutput { DictTypeName = a.DictType.Name });

        var dicts = new Dictionary<string, List<DictGetListOutput>>();
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
        // 获取字典类型信息
        var dictType = await _dictTypeRep.GetAsync(input.DictTypeId);
        if (dictType == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["字典类型不存在"]);
        }

        // 非树形结构强制重置ParentId=0
        if (!dictType.IsTree)
        {
            input.ParentId = 0;
        }
        // 树形结构需要验证ParentId有效性
        else if (input.ParentId > 0)
        {
            var parentExists = await _dictRep.Select.Where(a => a.Id == input.ParentId).AnyAsync();

            if (!parentExists)
            {
                throw ResultOutput.Exception(_adminLocalizer["父级字典不存在"]);
            }
        }

        if (await _dictRep.Select.AnyAsync(a => a.DictTypeId == input.DictTypeId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["字典名称已存在"]);
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
            var maxSortQuery = _dictRep.Select.Where(a => a.DictTypeId == input.DictTypeId);
            // 树形结构时在同一父节点下排序
            if (dictType.IsTree)
            {
                maxSortQuery = maxSortQuery.Where(a => a.ParentId == input.ParentId);
            }

            var sort = await maxSortQuery.MaxAsync(a => a.Sort);
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
        // 获取字典类型信息
        var dictType = await _dictTypeRep.GetAsync(input.DictTypeId);
        if (dictType == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["字典类型不存在"]);
        }

        // 非树形结构强制重置ParentId=0
        if (!dictType.IsTree)
        {
            input.ParentId = 0;
        }
        // 树形结构需要验证ParentId有效性
        else if (input.ParentId > 0)
        {
            var parentExists = await _dictRep.Select.Where(a => a.Id == input.ParentId).AnyAsync();

            if (!parentExists)
            {
                throw ResultOutput.Exception(_adminLocalizer["父级字典不存在"]);
            }
        }

        var entity = await _dictRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["字典不存在"]);
        }

        if (await _dictRep.Select.AnyAsync(a => a.Id != input.Id && a.DictTypeId == input.DictTypeId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["字典名称已存在"]);
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
        var dataList = await select.WhereDynamicFilter(input.DynamicFilter)
            .ToListAsync(a => new DictExport 
            { 
                DictTypeName = a.DictType.Name,
                ParentName = a.Parent.Name,
            });

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
        var newIdSet = new HashSet<long>();

        var importResult = await _iEHelper.ImportDataAsync<DictImport>(file, fileId, async (importResult) =>
        {
            // 检查数据
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

            // ================ 新增步骤1：预生成ID并构建名称映射 ================
            var nameIdMap = new Dictionary<(long, string), long>(); // (DictTypeId, Name) -> Id
            foreach (var importData in importDataList)
            {
                // 设置字典类型ID
                importData.DictTypeId = dictTypeList.Where(a => a.Name == importData.DictTypeName).Select(a => a.Id).FirstOrDefault();

                // 查找数据库中的字典项
                var dbItem = dictList.FirstOrDefault(a => a.DictTypeId == importData.DictTypeId && a.Name == importData.Name);

                if (dbItem != null)
                {
                    // 已存在：使用数据库ID
                    importData.Id = dbItem.Id;
                }
                else
                {
                    // 新数据：生成新ID
                    var newId = YitIdHelper.NextId();
                    importData.Id = newId;
                    newIdSet.Add(newId);
                }

                // 记录名称映射（仅当字典类型有效时）
                if (importData.DictTypeId > 0)
                {
                    var key = (importData.DictTypeId, importData.Name);
                    nameIdMap[key] = importData.Id;
                }
            }

            // ================ 新增步骤2：处理ParentId ================
            foreach (var importData in importDataList)
            {
                var rowIndex = importDataList.ToList().FindIndex(o => o.Equals(importData)) + 2;

                if (importData.DictTypeId <= 0)
                {
                    // 字典类型不存在
                    var errorInfo = new DataRowErrorInfo()
                    {
                        RowIndex = rowIndex,
                    };
                    errorInfo.FieldErrors.Add(_adminLocalizer["字典分类"], _adminLocalizer["{0}不存在", importData.DictTypeName]);
                    errorList.Add(errorInfo);
                    continue;
                }

                // 处理ParentId（优先从本次导入数据中查找）
                if (!string.IsNullOrEmpty(importData.ParentName))
                {
                    var parentKey = (importData.DictTypeId, importData.ParentName);

                    if (nameIdMap.TryGetValue(parentKey, out var parentId))
                    {
                        importData.ParentId = parentId;
                    }
                    else
                    {
                        // 从数据库中查找
                        var dbParent = dictList.FirstOrDefault(a =>
                            a.DictTypeId == importData.DictTypeId &&
                            a.Name == importData.ParentName);

                        importData.ParentId = dbParent?.Id ?? 0;

                        // 数据库中也找不到
                        if (importData.ParentId == 0)
                        {
                            var errorInfo = new DataRowErrorInfo()
                            {
                                RowIndex = rowIndex,
                            };
                            errorInfo.FieldErrors.Add(_adminLocalizer["上级字典名称"], _adminLocalizer["{0}不存在", importData.ParentName]);
                            errorList.Add(errorInfo);
                        }
                    }
                }
                else
                {
                    importData.ParentId = 0; // 无上级
                }

                // ================ 优化后的重复检查 ================
                // 同时检查数据库和本次导入数据
                if (duplicateAction == 1) // 仅当选择覆盖时才检查
                {
                    // 检查编码重复
                    if (!string.IsNullOrEmpty(importData.Code))
                    {
                        // 检查本次导入数据
                        var importDup = importDataList.FirstOrDefault(a =>
                            a != importData &&
                            a.DictTypeId == importData.DictTypeId &&
                            a.Code == importData.Code);

                        // 检查数据库
                        var dbDup = dictList.FirstOrDefault(a =>
                            a.DictTypeId == importData.DictTypeId &&
                            a.Code == importData.Code &&
                            a.Id != importData.Id); // 排除自身

                        if (importDup != null || dbDup != null)
                        {
                            var errorInfo = new DataRowErrorInfo()
                            {
                                RowIndex = rowIndex,
                            };
                            errorInfo.FieldErrors.Add(_adminLocalizer["字典编码"], _adminLocalizer["{0}已存在", importData.Code]);
                            errorList.Add(errorInfo);
                        }
                    }

                    // 检查值重复（逻辑同上）
                    if (!string.IsNullOrEmpty(importData.Value))
                    {
                        var importDup = importDataList.FirstOrDefault(a =>
                            a != importData &&
                            a.DictTypeId == importData.DictTypeId &&
                            a.Value == importData.Value);

                        var dbDup = dictList.FirstOrDefault(a =>
                            a.DictTypeId == importData.DictTypeId &&
                            a.Value == importData.Value &&
                            a.Id != importData.Id);

                        if (importDup != null || dbDup != null)
                        {
                            var errorInfo = new DataRowErrorInfo()
                            {
                                RowIndex = rowIndex,
                            };
                            errorInfo.FieldErrors.Add(_adminLocalizer["字典值"], _adminLocalizer["{0}已存在", importData.Value]);
                            errorList.Add(errorInfo);
                        }
                    }
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
            // 新增数据
            var insertList = importDataList
                .Where(a => a.Id > 0 && newIdSet.Contains(a.Id)) // 使用生成的ID
                .Select(a => a.Adapt<DictEntity>())
                .ToList();

            output.InsertCount = insertList.Count;
            await _dictRep.InsertAsync(insertList);

            // 更新数据
            var updateList = importDataList
                .Where(a => a.Id > 0 && !newIdSet.Contains(a.Id)) // 排除新生成的ID
                .ToList();

            if (duplicateAction == 1 && updateList.Count > 0)
            {
                var updateIds = updateList.Select(a => a.Id).ToList();
                var dbDataList = await _dictRep.Where(a => updateIds.Contains(a.Id)).ToListAsync();

                foreach (var dbItem in dbDataList)
                {
                    var importItem = updateList.First(a => a.Id == dbItem.Id);
                    importItem.Adapt(dbItem); // 使用Mapster更新实体
                }

                output.UpdateCount = dbDataList.Count;
                await _dictRep.UpdateAsync(dbDataList);
            }
        }

        return output;
    }
}