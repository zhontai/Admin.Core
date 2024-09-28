using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.View;
using ZhonTai.Admin.Resources;
using ZhonTai.Admin.Services.View.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;

namespace ZhonTai.Admin.Services.View;

/// <summary>
/// 视图服务
/// </summary>
[Order(100)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class ViewService : BaseService, IViewService, IDynamicApi
{
    private readonly IViewRepository _viewRep;
    private readonly AdminLocalizer _adminLocalizer;

    public ViewService(IViewRepository viewRep, AdminLocalizer adminLocalizer)
    {
        _viewRep = viewRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ViewGetOutput> GetAsync(long id)
    {
        var result = await _viewRep.GetAsync<ViewGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<List<ViewListOutput>> GetListAsync(string key)
    {
        var data = await _viewRep
            .WhereIf(key.NotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync<ViewListOutput>();

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(ViewAddInput input)
    {
        var entity = Mapper.Map<ViewEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _viewRep.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _viewRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(ViewUpdateInput input)
    {
        var entity = await _viewRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["视图不存在"]);
        }

        Mapper.Map(input, entity);
        await _viewRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        await _viewRep.DeleteAsync(m => m.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchDeleteAsync(long[] ids)
    {
        await _viewRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SoftDeleteAsync(long id)
    {
        await _viewRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>

    public async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _viewRep.SoftDeleteAsync(ids);
    }

    /// <summary>
    /// 同步
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SyncAsync(ViewSyncInput input)
    {
        //查询所有视图
        var views = await _viewRep.Select.ToListAsync();
        var names = views.Select(a => a.Name).ToList();
        var paths = views.Select(a => a.Path).ToList();

        //path处理
        foreach (var view in input.Views)
        {
            view.Path = view.Path?.Trim();
        }

        //批量插入
        {
            var inputViews = (from a in input.Views where !(paths.Contains(a.Path) || names.Contains(a.Name)) select a).ToList();
            if (inputViews.Count > 0)
            {
                var insertViews = Mapper.Map<List<ViewEntity>>(inputViews);
                foreach (var insertView in insertViews)
                {
                    if (insertView.Label.IsNull())
                    {
                        insertView.Label = insertView.Name;
                    }
                }
                insertViews = await _viewRep.InsertAsync(insertViews);
                views.AddRange(insertViews);
            }
        }

        //批量更新
        {
            var inputPaths = input.Views.Select(a => a.Path).ToList();
            var inputNames = input.Views.Select(a => a.Name).ToList();

            //修改
            var updateViews = (from a in views where inputPaths.Contains(a.Path) || inputNames.Contains(a.Name) select a).ToList();
            if (updateViews.Count > 0)
            {
                foreach (var view in updateViews)
                {
                    var inputView = input.Views.Where(a => a.Name == view.Name || a.Path == view.Path).FirstOrDefault();
                    if (view.Label.IsNull())
                    {
                        view.Label = inputView.Label ?? inputView.Name;
                    }
                    if (view.Description.IsNull())
                    {
                        view.Description = inputView.Description;
                    }
                    view.Name = inputView.Name;
                    view.Path = inputView.Path;
                    view.Enabled = true;
                }
            }

            //禁用
            var disabledViews = (from a in views where (a.Path.NotNull() || a.Name.NotNull()) && (!inputPaths.Contains(a.Path) || !inputNames.Contains(a.Name)) select a).ToList();
            if (disabledViews.Count > 0)
            {
                foreach (var view in disabledViews)
                {
                    view.Enabled = false;
                }
            }

            updateViews.AddRange(disabledViews);
            await _viewRep.UpdateDiy.SetSource(updateViews)
            .UpdateColumns(a => new { a.Label, a.Name, a.Path, a.Enabled, a.Description })
            .ExecuteAffrowsAsync();
        }
    }
}