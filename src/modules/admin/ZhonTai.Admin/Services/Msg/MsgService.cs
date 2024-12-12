using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Msg;
using ZhonTai.Admin.Resources;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Repositories;
using ZhonTai.Admin.Services.Msg.Dto;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core;

namespace ZhonTai.Admin.Services.Msg;

/// <summary>
/// 消息服务
/// </summary>
[Order(20)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class MsgService : BaseService, IDynamicApi
{
    private readonly AdminRepositoryBase<MsgEntity> _msgRep;
    private readonly AdminRepositoryBase<UserEntity> _userRep;
    private readonly AdminRepositoryBase<MsgUserEntity> _msgUserRep;
    private readonly AdminLocalizer _adminLocalizer;

    public MsgService(
        AdminRepositoryBase<MsgEntity> msgRep,
        AdminRepositoryBase<UserEntity> userRep,
        AdminRepositoryBase<MsgUserEntity> msgUserRep,
        AdminLocalizer adminLocalizer
    )
    {
        _msgRep = msgRep;
        _userRep = userRep;
        _msgUserRep = msgUserRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<MsgGetOutput> GetAsync(long id)
    {
        var output = await _msgRep.Select
        .WhereDynamic(id)
        .ToOneAsync<MsgGetOutput>();

        return output;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<MsgGetPageOutput>> GetPageAsync(PageInput<MsgGetPageInput> input)
    {
        var title = input.Filter?.Title;

        var list = await _msgRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(title.NotNull(), a => a.Title.Contains(title))
        .Count(out var total)
        .OrderByDescending(true, a => a.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync(a => new MsgGetPageOutput { TypeName = a.Type.Name });

        var data = new PageOutput<MsgGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 查询消息用户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<MsgGetMsgUserListOutput>> GetMsgUserListAsync([FromQuery] MsgGetMsgUserListInput input)
    {
        var list = await _userRep.Select.From<MsgUserEntity>()
            .InnerJoin(a => a.t2.UserId == a.t1.Id)
            .Where(a => a.t2.MsgId == input.MsgId)
            .WhereIf(input.Name.NotNull(), a => a.t1.Name.Contains(input.Name))
            .OrderByDescending(a => a.t1.Id)
            .ToListAsync(a=> new MsgGetMsgUserListOutput
            {
                IsRead = a.t2.IsRead,
                ReadTime = a.t2.ReadTime,
            });

        return list;
    }

    /// <summary>
    /// 添加消息用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task AddMsgUserAsync(MsgAddMsgUserListInput input)
    {
        var msgId = input.MsgId;
        var userIds = await _msgUserRep.Select.Where(a => a.MsgId == msgId).ToListAsync(a => a.UserId);
        var insertUserIds = input.UserIds.Except(userIds);

        if (insertUserIds != null && insertUserIds.Any())
        {
            var userMsgList = insertUserIds.Select(userId => new MsgUserEntity 
            { 
                UserId = userId, 
                MsgId = msgId 
            }).ToList();

            await _msgUserRep.InsertAsync(userMsgList);

            //推送消息
            var imConfig = AppInfo.GetOptions<ImConfig>();
            if (imConfig.Enable)
            {
                ImHelper.SendMessage(0, insertUserIds, new
                {
                    evts = new[]
                    {
                        new { name = "checkUnreadMsg", data = new { } }
                    }
                });
            }
        }
    }

    /// <summary>
    /// 移除消息用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task RemoveMsgUserAsync(MsgAddMsgUserListInput input)
    {
        var userIds = input.UserIds;
        if (userIds != null && userIds.Any())
        {
            await _msgUserRep.Where(a => a.MsgId == input.MsgId && input.UserIds.Contains(a.UserId)).ToDelete().ExecuteAffrowsAsync();
        }
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(MsgAddInput input)
    {
        var entity = Mapper.Map<MsgEntity>(input);
        entity.Status = MsgStatusEnum.Draft;
        await _msgRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(MsgUpdateInput input)
    {
        var entity = await _msgRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["消息不存在"]);
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
        //删除消息用户
        await _msgUserRep.DeleteAsync(a => a.MsgId == id);
        //删除消息
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
        //删除消息用户
        await _msgUserRep.DeleteAsync(a => ids.Contains(a.MsgId));
        //删除消息
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
        await _msgUserRep.DeleteAsync(a => a.MsgId == id);
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
        await _msgUserRep.DeleteAsync(a => ids.Contains(a.MsgId));
        await _msgRep.SoftDeleteAsync(a => ids.Contains(a.Id));
    }
}