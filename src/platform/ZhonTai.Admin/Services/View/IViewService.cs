using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.View.Dto;

namespace ZhonTai.Admin.Services.View
{
    /// <summary>
    /// 视图接口
    /// </summary>
    public interface IViewService
    {
        Task<IResultOutput> GetAsync(long id);

        Task<IResultOutput> GetListAsync(string key);

        Task<IResultOutput> AddAsync(ViewAddInput input);

        Task<IResultOutput> UpdateAsync(ViewUpdateInput input);

        Task<IResultOutput> DeleteAsync(long id);

        Task<IResultOutput> SoftDeleteAsync(long id);

        Task<IResultOutput> BatchSoftDeleteAsync(long[] ids);

        Task<IResultOutput> SyncAsync(ViewSyncInput input);
    }
}