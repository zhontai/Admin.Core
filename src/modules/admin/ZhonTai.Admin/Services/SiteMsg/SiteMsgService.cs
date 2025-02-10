using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Msg;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Repositories;
using ZhonTai.Admin.Services.SiteMsg.Dto;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Services.MsgType;
using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Resources;

namespace ZhonTai.Admin.Services.SiteMsg;

/// <summary>
/// 站内信服务
/// </summary>
[Order(20)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class SiteMsgService : BaseService, IDynamicApi
{
    private readonly AdminRepositoryBase<MsgUserEntity> _msgUserRep;
    private readonly MsgTypeService _msgTypeService;
    private readonly AdminLocalizer _adminLocalizer;

    public SiteMsgService(
        AdminRepositoryBase<MsgUserEntity> msgUserRep,
        MsgTypeService msgTypeService,
        AdminLocalizer adminLocalizer
    )
    {
        _msgUserRep = msgUserRep;
        _msgTypeService = msgTypeService;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 获得内容
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Login]
    [HttpGet]
    public async Task<SiteMsgGetContentOutput> GetContentAsync(long id)
    {
        if (!(id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["请选择消息"]);
        }

        var output = await _msgUserRep.Select
        .Where(a => a.UserId == User.Id)
        .Where(a => a.Id == id)
        .FirstAsync(a => new SiteMsgGetContentOutput
        {
            MsgId = a.MsgId,
            Title = a.Msg.Title,
            TypeName = a.Msg.Type.Name,
            Content = a.Msg.Content,
            ReceivedTime = a.CreatedTime,
            IsRead = a.IsRead,
        });

        return output;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Login]
    [HttpPost]
    public async Task<PageOutput<SiteMsgGetPageOutput>> GetPageAsync(PageInput<SiteMsgGetPageInput> input)
    {
        var title = input.Filter?.Title;
        var isRead = input.Filter?.IsRead;

        var select = _msgUserRep.Select
            .Where(a => a.UserId == User.Id)
            .WhereIf(isRead.HasValue, a => a.IsRead == isRead.Value)
            .WhereIf(title.NotNull(), a => a.Msg.Title.Contains(title));

        var typeId = input.Filter?.TypeId;
        if (typeId.HasValue)
        {
            var typeIdList = await _msgTypeService.GetChildIdListAsync(typeId.Value);
            select = select.Where(a => typeIdList.Contains(a.Msg.TypeId));
        }

        var list = await select
        .Count(out var total)
        .OrderByDescending(true, a => a.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync(a => new SiteMsgGetPageOutput 
        { 
            Title = a.Msg.Title,
            TypeId = a.Msg.TypeId,
            TypeName = a.Msg.Type.Name,
            IsRead = a.IsRead,
            ReceivedTime = a.CreatedTime,
        });

        var data = new PageOutput<SiteMsgGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 是否未读
    /// </summary>
    /// <returns></returns>
    [Login]
    [HttpGet]
    public async Task<bool> IsUnreadAsync()
    {
        var hasUnread = await _msgUserRep.Select.Where(a => a.UserId == User.Id && a.IsRead == false).AnyAsync();

        return hasUnread;
    }

    /// <summary>
    /// 全部标为已读
    /// </summary>
    /// <returns></returns>
    [Login]
    [HttpPost]
    public async Task SetAllReadAsync()
    {
        await _msgUserRep.UpdateDiy.Set(a => new MsgUserEntity
        {
            IsRead = true,
            ReadTime = DbHelper.ServerTime,
            ModifiedUserId = User.Id,
            ModifiedUserName = User.UserName,
            ModifiedUserRealName = User.Name,
            ModifiedTime = DbHelper.ServerTime,
        })
        .Where(a => a.UserId == User.Id)
        .Where(a => a.IsRead == false)
        .ExecuteAffrowsAsync();
    }

    /// <summary>
    /// 标为已读
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Login]
    [HttpPost]
    public async Task SetReadAsync(long id)
    {
        if (!(id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["请选择消息"]);
        }

        await _msgUserRep.UpdateDiy.Set(a => new MsgUserEntity
        {
            IsRead = true,
            ReadTime = DbHelper.ServerTime,
            ModifiedUserId = User.Id,
            ModifiedUserName = User.UserName,
            ModifiedUserRealName = User.Name,
            ModifiedTime = DbHelper.ServerTime,
        })
        .Where(a => a.Id == id)
        .Where(a => a.UserId == User.Id)
        .Where(a => a.IsRead == false)
        .ExecuteAffrowsAsync();
    }

    /// <summary>
    /// 批量标为已读
    /// </summary>
    /// <param name="idList"></param>
    /// <returns></returns>
    [Login]
    [HttpPost]
    public async Task BatchSetReadAsync(long[] idList)
    {
        if (!(idList?.Length > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["请选择消息"]);
        }

        await _msgUserRep.UpdateDiy.Set(a => new MsgUserEntity
        {
            IsRead = true,
            ReadTime = DbHelper.ServerTime,
            ModifiedUserId = User.Id,
            ModifiedUserName = User.UserName,
            ModifiedUserRealName = User.Name,
            ModifiedTime = DbHelper.ServerTime,
        })
        .Where(a => idList.Contains(a.Id))
        .Where(a => a.UserId == User.Id)
        .Where(a => a.IsRead == false)
        .ExecuteAffrowsAsync();
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Login]
    public virtual async Task SoftDeleteAsync(long id)
    {
        if (!(id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["请选择消息"]);
        }

        await _msgUserRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <returns></returns>
    [Login]
    [HttpPost]
    public async Task BatchSoftDeleteAsync(long[] ids)
    {
        if (!(ids?.Length > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["请选择消息"]);
        }

        await _msgUserRep.SoftDeleteAsync(a => ids.Contains(a.Id) && a.UserId == User.Id);
    }
}