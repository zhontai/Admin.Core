using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin.Core.Common.Output;
using Admin.Core.Service.Admin.Cache;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 缓存管理
    /// </summary>
    public class CacheController : AreaController
    {
        
        private readonly ICacheService _cacheServices;
        
        public CacheController(ICacheService cacheServices)
        {
            _cacheServices = cacheServices;
        }

        /// <summary>
        /// 获取缓存列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseOutput List()
        {
            return _cacheServices.List();
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> Clear(string cacheKey)
        {
            return await _cacheServices.ClearAsync(cacheKey);
        }
    }
}
