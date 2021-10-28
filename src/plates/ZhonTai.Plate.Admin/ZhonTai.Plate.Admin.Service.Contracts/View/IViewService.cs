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
        Task<IResponseOutput> GetAsync(long id);

        Task<IResponseOutput> ListAsync(string key);

        Task<IResponseOutput> PageAsync(PageInput<ViewEntity> model);

        Task<IResponseOutput> AddAsync(ViewAddInput input);

        Task<IResponseOutput> UpdateAsync(ViewUpdateInput input);

        Task<IResponseOutput> DeleteAsync(long id);

        Task<IResponseOutput> SoftDeleteAsync(long id);

        Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids);

        Task<IResponseOutput> SyncAsync(ViewSyncInput input);
    }
}