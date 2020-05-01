
using Admin.Core.Common.Input;
using Admin.Core.Service.Admin.Api.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using System.Threading.Tasks;

namespace Admin.Core.Service.Admin.Api
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IApiService
    {
        /// <summary>
        /// 获得一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetAsync(long id);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<IResponseOutput> ListAsync(string key);

        /// <summary>
        /// 获得分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IResponseOutput> PageAsync(PageInput<ApiEntity> model);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddAsync(ApiAddInput input);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateAsync(ApiUpdateInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> DeleteAsync(long id);

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> SoftDeleteAsync(long id);

        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids);

        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> SyncAsync(ApiSyncInput input);
    }
}
