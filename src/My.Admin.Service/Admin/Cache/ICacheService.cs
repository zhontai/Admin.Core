using My.Admin.Common.Output;
using System.Threading.Tasks;

namespace My.Admin.Service.Admin.Cache
{
    /// <summary>
    /// 缓存服务
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// 缓存列表
        /// </summary>
        /// <returns></returns>
        IResponseOutput List();

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        Task<IResponseOutput> ClearAsync(string cacheKey);
    }
}