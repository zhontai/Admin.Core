using ZhonTai.Admin.Services.View.Dto;

namespace ZhonTai.Admin.Services.View;

/// <summary>
/// 视图接口
/// </summary>
public interface IViewService
{
    Task<ViewGetOutput> GetAsync(long id);

    Task<List<ViewGetListOutput>> GetListAsync(ViewGetListInput input);

    Task<long> AddAsync(ViewAddInput input);

    Task UpdateAsync(ViewUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);

    Task SyncAsync(ViewSyncInput input);
}