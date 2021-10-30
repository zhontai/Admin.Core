using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Domain.View;
using ZhonTai.Plate.Admin.Service.View.Input;


namespace ZhonTai.Plate.Admin.Service.View
{
    /// <summary>
    /// 视图服务
    /// </summary>
    public interface IViewService
    {
        /// <summary>
        /// 获得一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResultOutput> GetAsync(long id);

        Task<IResultOutput> GetListAsync(string key);

        Task<IResultOutput> GetPageAsync(PageInput<ViewEntity> model);

        Task<IResultOutput> AddAsync(ViewAddInput input);

        Task<IResultOutput> UpdateAsync(ViewUpdateInput input);

        Task<IResultOutput> DeleteAsync(long id);

        Task<IResultOutput> SoftDeleteAsync(long id);

        Task<IResultOutput> BatchSoftDeleteAsync(long[] ids);

        Task<IResultOutput> SyncAsync(ViewSyncInput input);
    }
}