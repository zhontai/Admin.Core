using Magicodes.ExporterAndImporter.Excel.AspNetCore;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Resources;
using System;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Auth;
using ZhonTai.Admin.Tools.Cache;
using System.Collections.Generic;
using Magicodes.ExporterAndImporter.Core.Models;
using Microsoft.AspNetCore.Http;
using System.Text;
using ZhonTai.Admin.Services.Dict.Dto;
using System.Linq;

namespace ZhonTai.Admin.Core.Helpers;

/// <summary>
/// 导入导出帮助类
/// </summary>
[InjectSingleton]
public class IEHelper
{
    private readonly AdminLocalizer _adminLocalizer;
    private readonly IUser _user;
    private readonly ICacheTool _cache;

    public IEHelper(AdminLocalizer adminLocalizer,
        IUser user,
        ICacheTool cache) 
    {
        _adminLocalizer = adminLocalizer;
        _user = user;
        _cache = cache;
    }

    /// <summary>
    /// 下载模板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public async Task<ActionResult> DownloadTemplateAsync<T>(T type, string fileName) where T : class, new()
    {
        var result = await new ExcelImporter().GenerateTemplateBytes<T>();
        return new XlsxFileResult(result, fileName);
    }

    /// <summary>
    /// 下载错误标记文件
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public async Task<ActionResult> DownloadErrorMarkAsync(string fileId, string fileName)
    {
        var excelErrorMarkKey = CacheKeys.GetExcelErrorMarkKey(_user.Id, fileId);
        var fileStream = await _cache.GetAsync<byte[]>(excelErrorMarkKey);
        await _cache.DelAsync(excelErrorMarkKey);
        if (fileStream == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["请重新导入数据，再下载错误标记文件"], statusCode: 500);
        }

        if (fileName.IsNull())
        {
            fileName = _adminLocalizer["错误标记文件{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")];
        }

        return new XlsxFileResult(fileStream, fileName);
    }

    /// <summary>
    /// 导出数据
    /// </summary>
    /// <param name="dataItems"></param>
    /// <param name="fileName"></param>
    /// <param name="sheetName"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> ExportDataAsync<T>(ICollection<T> dataItems, string fileName = null, string sheetName = null) where T : class, new()
    {
        var result = await new ExcelExporter().Append(dataItems, sheetName).ExportAppendDataAsByteArray();

        if (fileName.IsNull())
        {
            fileName = _adminLocalizer["数据列表{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")];
        }

        return new XlsxFileResult(result, fileName);
    }

    /// <summary>
    /// 导入数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="file"></param>
    /// <param name="fileId"></param>
    /// <param name="importResultCallback"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ImportResult<T>> ImportDataAsync<T>(IFormFile file, string fileId, Func<ImportResult<T>, Task<ImportResult<T>>> importResultCallback = null) where T : class, new()
    {
        var importResult = await new ExcelImporter().Import<T>(file.OpenReadStream());

        if (importResultCallback != null)
        {
            importResult = await importResultCallback(importResult);
        }

        var errorMsg = new StringBuilder();
        if (importResult != null && importResult.HasError)
        {
            if (importResult.Exception != null)
            {
                errorMsg.AppendLine("错误信息：");
                errorMsg.AppendLine(importResult.Exception.Message);
            }

            if (importResult.TemplateErrors != null && importResult.TemplateErrors.Count > 0)
            {
                errorMsg.AppendLine("缺少数据列：" + string.Join("，", importResult.TemplateErrors.Select(m => m.RequireColumnName).ToList()));
            }
        }

        var rowErros = importResult.RowErrors;
        if (rowErros?.Count > 0)
        {
            errorMsg.AppendLine("数据填写有误：");
            rowErros = rowErros.OrderBy(a => a.RowIndex).ToList();
            foreach (DataRowErrorInfo drErrorInfo in rowErros)
            {
                foreach (var item in drErrorInfo.FieldErrors)
                {
                    errorMsg.AppendLine($"第{drErrorInfo.RowIndex}行 - {item.Key}：{item.Value}");
                }
            }

            //缓存错误标记文件
            new ExcelImporter().OutputBussinessErrorData<DictImport>(file.OpenReadStream(), rowErros.ToList(), out byte[] fileByte);
            var userId = _user.Id;
            await _cache.DelAsync(CacheKeys.GetExcelErrorMarkKey(userId, fileId));
            await _cache.SetAsync(CacheKeys.GetExcelErrorMarkKey(userId, fileId), fileByte, TimeSpan.FromMinutes(20));
        }

        if (errorMsg.Length > 0)
        {
            throw ResultOutput.Exception(errorMsg.ToString());
        }

        return importResult;
    }
}