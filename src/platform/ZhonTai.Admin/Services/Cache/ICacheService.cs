using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;

namespace ZhonTai.Admin.Services.Cache
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// 缓存列表
        /// </summary>
        /// <returns></returns>
        IResultOutput GetList();

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        Task<IResultOutput> ClearAsync(string cacheKey);
    }
}