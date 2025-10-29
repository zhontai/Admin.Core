using Microsoft.AspNetCore.Mvc;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.DynamicApi;

namespace ZhonTai.Module.Dev.Services.DevProject;

/// <summary>
/// 项目服务
/// </summary>
public partial class DevProjectService : IDynamicApi
{
    /// <summary>
    /// 批量生成
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="groupId">模板组</param>
    /// <returns></returns>
    [HttpPost]
    public async Task BatchGenerateAsync(long[] ids, long groupId)
    {
        foreach (var id in ids)
        {
            await GenerateAsync(id, groupId);
        }
    }

    /// <summary>
    /// 生成
    /// </summary>
    /// <param name="id">项目ID</param>
    /// <param name="groupId">模板组</param>
    /// <returns></returns>
    [HttpGet]
    public async Task GenerateAsync(long id, long groupId)
    {
        //项目
        var project = await _devProjectRep
            .Where(w => w.Id == id)
            .FirstAsync();
        //模型
        var models = _devProjectModelRep.Where(s => s.ProjectId == project.Id).ToList();
        //字段
        var fields = _devProjectModelFieldRep.Where(s => models.Any(s2 => s2.Id == s.ModelId)).ToList();
        //模板
        var templates = _devTemplateRep.Where(s => s.GroupId == groupId).ToList();
        try
        {
            RazorEngine.Engine.Razor = RazorEngineService.Create(new RazorEngine.Configuration.
                TemplateServiceConfiguration
            {
                EncodedStringFactory = new RazorEngine.Text.RawStringFactory()// Raw string encoding.
            });
        }
        catch (Exception ex)
        {
            RazorEngine.Engine.Razor?.Dispose();
            throw ResultOutput.Exception(msg: "初始化编译器失败：" + ex.Message);
        }

        var errors = new List<string>();
        try
        {
            //一个个模型生成
            foreach (var model in models)
            {
                var modelFields = fields.Where(s => s.ModelId == model.Id);
                //模型渲染
                var gen = new Dictionary<string, object>();
                gen.Add("project", project);
                gen.Add("model", model);
                gen.Add("modeeFields", modelFields);
                //模型是否禁用
                if (!model.IsEnable) continue;
                foreach (var tpl in templates)
                {
                    //模板是否禁用
                    if (!tpl.IsEnable) continue;
                    if (string.IsNullOrEmpty(tpl.OutTo))
                    {
                        errors.Add($"生成【{tpl.Name}】输出路径为空");
                        break;
                    }
                    var outPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tpl.OutTo);
                    var outDir = Path.GetDirectoryName(outPath);

                    var codeText = tpl.Content;
                    if (!Directory.Exists(outDir))
                        Directory.CreateDirectory(outDir);
                    try
                    {
                        if (Path.Exists(outPath))
                        {
                            Trace.WriteLine($"文件存在：{outPath}");
                            continue;
                        }
                        _RazorCompile(RazorEngine.Engine.Razor, gen, $"{project.Code}_{model.Code}_{tpl.Name}.tpl", codeText, outPath);
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"生成【{tpl.Name}】时发生错误：{ex.Message}");
                    }
                }
            }
            if (errors.Count > 0)
                throw ResultOutput.Exception(string.Join("\n", errors));
        }
        catch (Exception ex)
        {
            throw ResultOutput.Exception(ex.Message);
        }
        finally
        {
            RazorEngine.Engine.Razor.Dispose();
        }
    }
    void _RazorCompile(IRazorEngineService razor, Dictionary<string, object> genDic, string key, string code, string outfile)
    {
        if (razor == null) return;

        //razor.Compile(new LoadedTemplateSource(code), key);
        using (var fs = new FileStream(outfile, FileMode.Create, FileAccess.Write))
        {
            using (var fw = new StreamWriter(fs, Encoding.UTF8))
            {
                try
                {
                    razor.RunCompile(new LoadedTemplateSource(code), key, fw, genDic.GetType(), genDic);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(ex.Message);
                }
            }
        }
    }
}