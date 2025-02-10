using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Reflection;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Common.Helpers;
using Microsoft.Extensions.Options;

namespace ZhonTai.Admin.Services.Cache;

/// <summary>
/// 缓存服务
/// </summary>
[Order(80)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class CacheService : BaseService, ICacheService, IDynamicApi
{
    private readonly AppConfig _appConfig;
    public CacheService(IOptions<AppConfig> appConfig)
    {
        _appConfig = appConfig.Value;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <returns></returns>
    public List<dynamic> GetList()
    {
        var list = new List<dynamic>();

        Assembly[] assemblies = AssemblyHelper.GetAssemblyList(_appConfig.AssemblyNames);

        foreach (Assembly assembly in assemblies)
        {
            var types = assembly.GetExportedTypes().Where(a => a.GetCustomAttribute<ScanCacheKeysAttribute>(false) != null);
            foreach (Type type in types)
            {
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                foreach (FieldInfo field in fields)
                {
                    var descriptionAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

                    list.Add(new
                    {
                        field.Name,
                        Value = field.GetRawConstantValue().ToString(),
                        descriptionAttribute?.Description
                    });
                }
            }
        }

        return list;
    }

    /// <summary>
    /// 清除缓存
    /// </summary>
    /// <param name="cacheKey">缓存键</param>
    /// <returns></returns>
    public async Task ClearAsync(string cacheKey)
    {
        Logger.LogWarning($"{User.Id}.{User.UserName}清除缓存[{cacheKey}]");
        await Cache.DelByPatternAsync(cacheKey + "*");
    }
}