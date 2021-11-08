using ZhonTai.Common.Domain.Dto;
using System.Threading.Tasks;
using ZhonTai.Plate.Admin.Service.Api.Input;
using ZhonTai.Plate.Admin.Domain.Api.Dto;

namespace ZhonTai.Plate.Admin.Service.Api
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
        Task<IResultOutput> GetAsync(long id);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<IResultOutput> GetListAsync(string key);

        /// <summary>
        /// 获得分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResultOutput> GetPageAsync(PageInput<ApiGetPageDto> input);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResultOutput> AddAsync(ApiAddInput input);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResultOutput> UpdateAsync(ApiUpdateInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResultOutput> DeleteAsync(long id);

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResultOutput> SoftDeleteAsync(long id);

        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IResultOutput> BatchSoftDeleteAsync(long[] ids);

        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResultOutput> SyncAsync(ApiSyncInput input);
    }
}