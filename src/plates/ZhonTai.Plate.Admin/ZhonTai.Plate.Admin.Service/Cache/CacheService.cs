using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Service.Contracts;
using ZhonTai.Tools.DynamicApi;
using ZhonTai.Tools.DynamicApi.Attributes;

namespace ZhonTai.Plate.Admin.Service.Cache
{
    /// <summary>
    /// 缓存服务
    /// </summary>
    [DynamicApi(Area = "admin")]
    public class CacheService : BaseService, ICacheService, IDynamicApi
    {
        public CacheService()
        {
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        public IResultOutput GetList()
        {
            var list = new List<object>();
            var cacheKey = typeof(CacheKey);
            var fields = cacheKey.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (var field in fields)
            {
                var descriptionAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

                list.Add(new
                {
                    field.Name,
                    Value = field.GetRawConstantValue().ToString(),
                    descriptionAttribute?.Description
                });
            }

            return ResultOutput.Ok(list);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <returns></returns>
        public async Task<IResultOutput> ClearAsync(string cacheKey)
        {
            Logger.LogWarning($"{User.Id}.{User.Name}清除缓存[{cacheKey}]");
            await Cache.DelByPatternAsync(cacheKey);
            return ResultOutput.Ok();
        }
    }
}