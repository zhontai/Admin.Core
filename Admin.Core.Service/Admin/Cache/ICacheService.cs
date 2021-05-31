

using Admin.Core.Common.Output;
using System.Threading.Tasks;

namespace Admin.Core.Service.Admin.Cache
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
