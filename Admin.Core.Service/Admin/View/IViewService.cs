
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.View.Input;


namespace Admin.Core.Service.Admin.View
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
